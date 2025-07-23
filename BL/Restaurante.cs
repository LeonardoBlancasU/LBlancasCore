using DL;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ML;
using System.Diagnostics.CodeAnalysis;

namespace BL
{
    public class Restaurante
    {
        private readonly LblancasCoreContext _context;
        public Restaurante (LblancasCoreContext context)
        {
            _context = context;
        }
        public ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();
            try 
            { 
                var query = _context.Restaurantes.FromSqlRaw("RestauranteGetAll").ToList();

                if(query.Count > 0)
                {
                    foreach(var item in query)
                    {
                        ML.Restaurante restaurante = new ML.Restaurante();
                        restaurante.IdRestaurante = item.IdRestaurante;
                        restaurante.Nombre = item.Nombre;
                        restaurante.Slogan = item.Slogan;
                        restaurante.Descripcion = item.Descripcion;
                        restaurante.Imagen = item.Imagen;
                        result.Objects.Add(restaurante);
                    }
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontraron Restaurantes";
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
        public ML.Result Add(ML.Restaurante restaurante)
        {
            ML.Result result = new ML.Result();
            try
            {
                var Nombre = new SqlParameter("@Nombre", restaurante.Nombre ?? (object)DBNull.Value);
                var Slogan = new SqlParameter("@Slogan", restaurante.Slogan ?? (object)DBNull.Value);
                var Descripcion = new SqlParameter("@Descripcion", restaurante.Descripcion ?? (object)DBNull.Value);
                var Imagen = new SqlParameter("@Imagen", restaurante.Imagen ?? (object)DBNull.Value);
                var query = _context.Database.ExecuteSqlRaw("RestauranteAdd @Nombre, @Slogan, @Imagen, @Descripcion", Nombre, Slogan, Imagen, Descripcion);
                if(query > 0)
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se pudo agregar el Restaurante";
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public ML.Result Update(ML.Restaurante restaurante)
        {
            ML.Result result = new ML.Result();
            try
            {
                var Nombre = new SqlParameter("@Nombre", restaurante.Nombre ?? (object)DBNull.Value);
                var Slogan = new SqlParameter("@Slogan", restaurante.Slogan ?? (object)DBNull.Value);
                var Descripcion = new SqlParameter("@Descripcion", restaurante.Descripcion ?? (object)DBNull.Value);
                var Imagen = new SqlParameter("@Imagen", restaurante.Imagen ?? (object)DBNull.Value);
                var Id = new SqlParameter("@IdRestaurante", restaurante.IdRestaurante);
                var query = _context.Database.ExecuteSqlRaw("RestauranteUpdate @IdRestaurante, @Nombre, @Slogan, @Imagen, @Descripcion", Id,Nombre, Slogan, Imagen, Descripcion);
                if (query > 0)
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se pudo actualizar el Restaurante";
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
        public ML.Result Delete(int IdRestaurante)
        {
            ML.Result result = new ML.Result();
            try
            {
                var Id = new SqlParameter("@IdRestaurante", IdRestaurante);
                var query = _context.Database.ExecuteSqlRaw("RestauranteDelete @IdRestaurante", Id);
                if (query > 0)
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se pudo eliminar el Restaurante";
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
        public ML.Result GetById(int IdRestaurante)
        {
            ML.Result result = new ML.Result();
            try
            {
                var Id = new SqlParameter("@IdRestaurante", IdRestaurante);
                var query = _context.Restaurantes.FromSqlRaw("RestauranteGetById @IdRestaurante", Id).AsEnumerable().FirstOrDefault();
                if (query != null)
                {
                    ML.Restaurante restaurante = new ML.Restaurante();
                    restaurante.Nombre = query.Nombre;
                    restaurante.Slogan = query.Slogan;
                    restaurante.Descripcion = query.Descripcion;
                    restaurante.Imagen = query.Imagen;
                    restaurante.IdRestaurante = query.IdRestaurante;
                    result.Object = restaurante;
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se pudo actualizar el Restaurante";
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
