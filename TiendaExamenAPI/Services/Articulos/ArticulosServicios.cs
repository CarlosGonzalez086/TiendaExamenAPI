using TiendaExamenAPI.DbData.DtoModels.articulos;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.DbData.Repository.Articulos;
using TiendaExamenAPI.DbData.Repository.Tienda;
using TiendaExamenAPI.Services.FuncionesGenerales;

namespace TiendaExamenAPI.Services.Articulos
{
    public class ArticulosServicios
    {
        private readonly ArticulosRepositorio _repositorio;
        private readonly ImageStorageService _storageService;

        public ArticulosServicios(
            ArticulosRepositorio repositorio,
            ImageStorageService storageService)
        {
            _repositorio = repositorio;
            _storageService = storageService;
        }
        public async Task<Response> GuardarArticuloAsync(dtoArticulos info)
        {
            var validacion = ValidarArticulo(info, esActualizacion: false);
            if (validacion != null) return validacion;

            if (!string.IsNullOrEmpty(info.imagen))
            {
                var imgResult = await _storageService.SaveImageAsync(null, info.imagen);
                if (imgResult.codigo == "000")
                {
                    info.imagen = imgResult.respuesta.ToString();
                }
            }

            bool ok = await _repositorio.InsertadoAsync(info);

            return ok
                ? ResponseOk("Articulo registrado correctamente")
                : ResponseError("900", "Ha ocurrido un error al registrar");
        }

        public async Task<Response> ActualizarArticuloAsync(dtoArticulos info)
        {
            if (info.id <= 0)
            {
                return ResponseError("901", "ID inválido");
            }

            var validacion = ValidarArticulo(info, esActualizacion: true);
            if (validacion != null) return validacion;

            if (!string.IsNullOrEmpty(info.imagen))
            {
                var imgResult = await _storageService.SaveImageAsync(null, info.imagen);
                if (imgResult.codigo == "000")
                {
                    info.imagen = imgResult.respuesta.ToString();
                }
            }

            bool ok = await _repositorio.ActualizacionAsync(info, info.id);

            return ok
                ? ResponseOk("Guardado correctamente")
                : ResponseError("900", "Ha ocurrido un error al actualizar");
        }

        public async Task<Response> ObtenerArticuloAsync(long id)
        {
            if (id <= 0)
            {
                return ResponseError("901", "Parámetros incorrectos");
            }

            var articulo = await _repositorio.ObtenerPorIdAsync(id);

            if (articulo == null)
            {
                return new Response
                {
                    codigo = "404",
                    mensaje = "Artículo no encontrado",
                    respuesta = ""
                };
            }

            return new Response
            {
                codigo = "200",
                mensaje = "Información del artículo",
                respuesta = articulo
            };
        }


        public async Task<Response> EliminarArticuloAsync(long id)
        {
            if (id <= 0)
            {
                return ResponseError("901", "Parámetros incorrectos");
            }

            bool ok = await _repositorio.EliminacionAsync(id);

            return ok
                ? ResponseOk("Eliminado correctamente")
                : ResponseError("900", "Error al eliminar");
        }


        public async Task<Response> ListaAsync(int take, int skip)
        {
            take = take == 0 ? 99999 : take;

            var result = await _repositorio.ListaAsync(take, skip);

            int totalPages = result.total == 0
                ? 0
                : (int)Math.Ceiling((double)result.total / take);

            return new Response
            {
                codigo = "200",
                mensaje = "",
                respuesta = new
                {
                    data = result.data,
                    totalRows = result.total,
                    totalPages
                }
            };
        }

        private static Response? ValidarArticulo(
            dtoArticulos info,
            bool esActualizacion)
        {
            if (string.IsNullOrEmpty(info.codigo))
                return ResponseError("901", "El campo código es requerido");

            if (string.IsNullOrEmpty(info.descripcion))
                return ResponseError("901", "El campo descripción es requerido");

            if (info.precio <= 0)
                return ResponseError("901", "El precio debe ser mayor a 0");

            if (info.stock < 0)
                return ResponseError("901", "El stock no puede ser negativo");

            return null;
        }
        public async Task<Response> ObtenerArticulosAsync()
        {
            var lista = await _repositorio.ObtenerTodosAsync();

            return await Task.FromResult(new Response
            {
                codigo = "200",
                mensaje = "Listado de tiendas",
                respuesta = lista
            });
        }

        private static Response ResponseOk(string mensaje) =>
            new()
            {
                codigo = "200",
                mensaje = mensaje,
                respuesta = ""
            };

        private static Response ResponseError(string codigo, string mensaje) =>
            new()
            {
                codigo = codigo,
                mensaje = mensaje,
                respuesta = ""
            };
    }
}
