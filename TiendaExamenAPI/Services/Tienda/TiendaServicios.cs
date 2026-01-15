using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.DbData.DtoModels.Tienda;
using TiendaExamenAPI.DbData.Repository.Tienda;

namespace TiendaExamenAPI.Services.Tienda
{
    public class TiendaServicios
    {
        private readonly TiendaRepositorio _tiendaRepositorio;

        public TiendaServicios(TiendaRepositorio tiendaRepositorio)
        {
            _tiendaRepositorio = tiendaRepositorio;
        }
        public async Task<Response> GuardarTiendaAsync(dtoTienda tiendaInfo)
        {
            if (string.IsNullOrWhiteSpace(tiendaInfo.sucursal))
                return await Task.FromResult(Error("901", "El campo sucursal es requerido"));

            if (string.IsNullOrWhiteSpace(tiendaInfo.direccion))
                return await Task.FromResult(Error("901", "El campo dirección es requerido"));

            bool insertOk = await _tiendaRepositorio.InsertadoAsync(tiendaInfo);

            if (!insertOk)
            {
                return await Task.FromResult(new Response
                {
                    codigo = "900",
                    mensaje = "Error al registrar la tienda"
                });
            }

            return await Task.FromResult(new Response
            {
                codigo = "200",
                mensaje = "Tienda registrada correctamente"
            });
        }
        public async Task<Response> ActualizarTiendaAsync(dtoTienda tiendaInfo)
        {
            if (tiendaInfo.id == 0)
                return await Task.FromResult(Error("901", "Id inválido"));

            if (string.IsNullOrWhiteSpace(tiendaInfo.sucursal))
                return await Task.FromResult(Error("901", "El campo sucursal es requerido"));

            if (string.IsNullOrWhiteSpace(tiendaInfo.direccion))
                return await Task.FromResult(Error("901", "El campo dirección es requerido"));

            bool actualizado = await _tiendaRepositorio.ActualizacionAsync(tiendaInfo, tiendaInfo.id);

            if (!actualizado)
            {
                return await Task.FromResult(new Response
                {
                    codigo = "900",
                    mensaje = "Error al actualizar la tienda"
                });
            }

            return await Task.FromResult(new Response
            {
                codigo = "200",
                mensaje = "Guardado correctamente"
            });
        }

        public async Task<Response> ObtenerTiendaAsync(long id)
        {
            if (id == 0)
                return await Task.FromResult(Error("901", "Id inválido"));

            dtoTienda tienda = await _tiendaRepositorio.ObtenerPorIdAsync(id);

            if (tienda == null)
            {
                return await Task.FromResult(new Response
                {
                    codigo = "404",
                    mensaje = "Tienda no encontrada"
                });
            }

            return await Task.FromResult(new Response
            {
                codigo = "200",
                mensaje = "Información de la tienda",
                respuesta = tienda
            });
        }

        public async Task<Response> EliminarTiendaAsync(long id)
        {
            if (id == 0)
                return await Task.FromResult(Error("901", "Parámetros incorrectos"));

            bool eliminado = await _tiendaRepositorio.EliminacionAsync(id);

            if (!eliminado)
            {
                return await Task.FromResult(new Response
                {
                    codigo = "900",
                    mensaje = "Error al eliminar la tienda"
                });
            }

            return await Task.FromResult(new Response
            {
                codigo = "200",
                mensaje = "Eliminado correctamente"
            });
        }

        private static Response Error(string codigo, string mensaje) =>
            new()
            {
                codigo = codigo,
                mensaje = mensaje,
                respuesta = ""
            };
        public async Task<Response> ObtenerTiendasAsync()
        {
            var lista = await _tiendaRepositorio.ObtenerTodosAsync();

            return await Task.FromResult(new Response
            {
                codigo = "200",
                mensaje = "Listado de tiendas",
                respuesta = lista
            });
        }
    }
}
