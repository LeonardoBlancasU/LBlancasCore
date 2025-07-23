using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

namespace PL.Controllers
{
    public class RestauranteController : Controller
    {
        private readonly BL.Restaurante _BLRestaurante;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public RestauranteController(BL.Restaurante BLRestaurante, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _BLRestaurante = BLRestaurante;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Restaurante restaurante = new ML.Restaurante();
            //ML.Result result = _BLRestaurante.GetAll();
            ML.Result result = GetAllRest();
            if (result.Correct)
            {
                restaurante.Restaurantes = result.Objects;
            }
            return View(restaurante);
        }
        [HttpGet]
        public IActionResult Formulario(int? IdRestaurante)
        {
            ML.Restaurante restaurante = new ML.Restaurante();
            if (IdRestaurante > 0)
            {
                //ML.Result result = _BLRestaurante.GetById(IdRestaurante.Value);
                ML.Result result = GetByIdRest(IdRestaurante.Value);
                if (result.Correct)
                {
                    restaurante = result?.Object as ML.Restaurante ?? new ML.Restaurante();
                }
            }
            return View(restaurante);
        }
        [HttpPost]
        public IActionResult Formulario(ML.Restaurante restaurante, IFormFile ImagenFile)
        {
            ML.Result result = new ML.Result();
            if(ImagenFile != null && ImagenFile.Length > 0)
            {
                MemoryStream target = new MemoryStream();
                ImagenFile.CopyTo(target);
                byte[] data = target.ToArray();
                restaurante.Imagen = data;
            }
            else
            {
                // Si no se selecciona una imagen, puedes asignar una imagen predeterminada
                string defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Img", "Default.png");
                byte[] defaultImageData = System.IO.File.ReadAllBytes(defaultImagePath);
                restaurante.Imagen = defaultImageData;
            }
            if(restaurante.IdRestaurante == 0)
            {
                //result = _BLRestaurante.Add(restaurante);
                result = AddRest(restaurante);
                if (result.Correct)
                {
                    TempData["Agregado"] = "Restaurante agregado correctamente.";
                    return RedirectToAction("GetAll");
                }
                else
                {
                    TempData["Error"] = "Error al agregar el restaurante: " + result.ErrorMessage;
                }
            }
            else
            {
                //result = _BLRestaurante.Update(restaurante);
                result = UpdateRest(restaurante);
                if (result.Correct)
                {
                    TempData["Agregado"] = "Restaurante actualizado correctamente.";
                    return RedirectToAction("GetAll");
                }
                else
                {
                    TempData["Error"] = "Error al actualizar el restaurante: " + result.ErrorMessage;
                    return RedirectToAction("GetAll");
                }
            }
            return View(restaurante);
        }

        [HttpGet]
        public IActionResult Delete(int IdRestaurante)
        {
            //ML.Result result = _BLRestaurante.Delete(IdRestaurante);
            ML.Result result = DeleteRest(IdRestaurante);
            if (result.Correct)
            {
                TempData["Success"] = "Restaurante eliminado correctamente.";
            }
            else
            {
                TempData["Error"] = "Error al eliminar el restaurante: " + result.ErrorMessage;
            }
            return RedirectToAction("GetAll");
        }
        [NonAction]
        public ML.Result GetAllRest()
        {
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();
            string url = _configuration["AppSettings:Url"] ?? "";
            try 
            {
                using(var cliente = new HttpClient())
                {
                    cliente.BaseAddress = new Uri(url);
                    var responseTask = cliente.GetAsync("GetAll");

                    responseTask.Wait();

                    var request = responseTask.Result;
                    if (request.IsSuccessStatusCode)
                    {
                        var readTask = request.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        foreach(var item in readTask.Result.Objects?? new List<object>())
                        {
                            ML.Restaurante restaurante = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Restaurante>(item.ToString());
                            result.Objects.Add(restaurante);
                        }
                        result.Correct= true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }  //ML.Restaurante restaurante = (item as JObject)?.ToObject<ML.Restaurante>()?? new ML.Restaurante();
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        [NonAction]
        public ML.Result AddRest(ML.Restaurante restaurante)
        {
            ML.Result result = new ML.Result();
            string url = _configuration["AppSettings:Url"] ?? ""; ;
            try
            {
                using(var cliente = new HttpClient())
                {
                    cliente.BaseAddress = new Uri(url);

                    var postTask = cliente.PostAsJsonAsync<ML.Restaurante>("Add", restaurante);
                    postTask.Wait();

                    var request = postTask.Result;
                    if (request.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        [NonAction]
        public ML.Result UpdateRest(ML.Restaurante restaurante)
        {
            ML.Result result = new ML.Result();
            string url = _configuration["AppSettings:Url"] ?? "";
            try
            {
                using(var cliente = new HttpClient())
                {
                    cliente.BaseAddress = new Uri(url);

                    var postTask = cliente.PutAsJsonAsync<ML.Restaurante>("Update", restaurante);
                    postTask.Wait();

                    var request = postTask.Result;
                    if(request.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        [NonAction]
        public ML.Result DeleteRest(int IdRestaurante)
        {
            ML.Result result = new ML.Result();
            string url = _configuration["AppSettings:Url"] ?? "";
            try
            {
                using(var cliente = new HttpClient())
                {
                    cliente.BaseAddress = new Uri(url);

                    var postTask = cliente.DeleteAsync("Delete?IdRestaurante=" + IdRestaurante);
                    postTask.Wait();

                    var request = postTask.Result;
                    if(request.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        [NonAction]
        public ML.Result GetByIdRest(int IdRestaurante)
        {
            ML.Result result = new ML.Result();
            string url = _configuration["AppSettings:Url"] ?? "";
            try
            {
                using( var cliente = new HttpClient())
                {
                    cliente.BaseAddress = new Uri(url);

                    var responseTask = cliente.GetAsync("GetById?IdRestaurante=" + IdRestaurante);
                    responseTask.Wait();

                    var request = responseTask.Result;
                    if( request.IsSuccessStatusCode ) 
                    {
                        var readTask = request.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        ML.Restaurante restaurante = new ML.Restaurante();
                        restaurante = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Restaurante>(readTask.Result.Object.ToString());
                        result.Object = restaurante;
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}
