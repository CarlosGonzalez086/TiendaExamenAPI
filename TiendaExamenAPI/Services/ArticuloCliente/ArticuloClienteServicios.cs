using TiendaExamenAPI.DbData.DtoModels.ArticuloCliente;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.DbData.Repository.ArticuloCliente;
using TiendaExamenAPI.Services.Utils;

namespace TiendaExamenAPI.Services.ArticuloCliente
{
    public class ArticuloClienteServicios
    {
        private readonly ArticuloClienteRepositorio _repositorio;
        private readonly Context _context;

        public ArticuloClienteServicios(
            ArticuloClienteRepositorio repositorio,
            Context context)
        {
            _repositorio = repositorio;
            _context = context;
        }
        public async Task<Response> GuardarArticuloClienteAsync(
            dtoArticuloCliente info)
        {
            if (info.articulos == null || info.articulos.Count == 0)
            {
                return ResponseError("901", "Debes tener al menos 1 producto");
            }

            if (info.total <= 0)
            {
                return ResponseError("901", "El total debe ser mayor a 0");
            }

            var result = await _repositorio.InsertadoAsync(info.total);
            if (!result.success)
            {
                return ResponseError("901", "Error al registrar la compra");
            }


            foreach (var detalle in info.articulos)
            {
                var ok = await _repositorio.InsertadoDetalleAsync(
                    (int)result.insertedId,
                    detalle.articulo_id,
                    info.cliente_id,
                    detalle.cantidad);

                if (!ok)
                {
                    return ResponseError("901", "Error al registrar los productos");
                }
            }

            return new Response
            {
                codigo = "200",
                mensaje = "Compra completada",
                respuesta = ""
            };
        }

        public async Task<Response> ObtenerCompraClienteAsync(long id)
        {
            if (id <= 0)
            {
                return ResponseError("901", "Parámetros incorrectos");
            }

            var compra = await _repositorio.ObtenerCompraClienteAsync(id);

            if (compra == null)
            {
                return new Response
                {
                    codigo = "404",
                    mensaje = "No se encontró información",
                    respuesta = ""
                };
            }

            return new Response
            {
                codigo = "200",
                mensaje = "",
                respuesta = new { data = compra }
            };
        }

        public async Task<Response> ListaAsync(int take, int skip)
        {
            take = take == 0 ? 99999 : take;
            var clienteId = _context.getID();

            var result = await _repositorio.ListaAsync(
                take,
                skip,
                clienteId);

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

        private static Response ResponseError(string codigo, string mensaje)
        {
            return new Response
            {
                codigo = codigo,
                mensaje = mensaje,
                respuesta = ""
            };
        }
    }
}
