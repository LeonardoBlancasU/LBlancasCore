using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestauranteController : ControllerBase
    {
        private readonly BL.Restaurante _BLRestaurante;
        private readonly IConfiguration _configuration;
        public RestauranteController (BL.Restaurante BLRestaurante, IConfiguration configuration)
        {
            _BLRestaurante = BLRestaurante;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll() 
        {
            ML.Result result = _BLRestaurante.GetAll();
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(ML.Restaurante restaurante) 
        {
            ML.Result result =_BLRestaurante.Add(restaurante);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(ML.Restaurante restaurante)
        {
            ML.Result result = _BLRestaurante.Update(restaurante);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(int IdRestaurante)
        {
            ML.Result result = _BLRestaurante.Delete(IdRestaurante);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(int IdRestaurante)
        {
            ML.Result result = _BLRestaurante.GetById(IdRestaurante);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }
    }
}
