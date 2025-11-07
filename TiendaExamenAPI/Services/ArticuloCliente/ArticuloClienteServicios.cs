using Newtonsoft.Json;
using System.Data;
using TiendaExamenAPI.DbData.DtoModels.ArticuloCliente;
using TiendaExamenAPI.DbData.DtoModels.articulos;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.DbData.Repository.ArticuloCliente;
using TiendaExamenAPI.Services.Utils;

namespace TiendaExamenAPI.Services.ArticuloCliente
{
    public class ArticuloClienteServicios
    {
        ArticuloClienteRepositorio articuloCliente = new();
        Context context = new();
        public async Task<Response> guardarArticuloCliente(dtoArticuloCliente articuloClienteInfo) 
        {
            if (articuloClienteInfo.articulos.Count == 0)
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "Debes tener al menos 1 producto para la compra",
                    respuesta = ""
                };
            }
            if (articuloClienteInfo.total == 0)
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El total debe ser mayor a 0",
                    respuesta = ""
                };
            }

            var result = articuloCliente.insertado(articuloClienteInfo.total);

            if (result.success)
            {
                bool todoCorrecto = false;
                foreach (dtoArticuloClienteDetalle clienteDetalle in articuloClienteInfo.articulos)                 
                {
                    bool correcto = await articuloCliente.insertadoDetalle(result.insertedId, clienteDetalle.articulo_id, articuloClienteInfo.cliente_id,clienteDetalle.cantidad);
                    if (correcto)
                    {
                        todoCorrecto = true;
                    }
                }
                if (todoCorrecto)
                {
                    return new Response
                    {
                        codigo = "200",
                        mensaje = "Compra completada",
                        respuesta = ""
                    };
                }
                else
                {
                    return new Response
                    {
                        codigo = "901",
                        mensaje = "Error en la compra",
                        respuesta = ""
                    };
                }
            }
            return new Response
            {
                codigo = "901",
                mensaje = "Error en la compra",
                respuesta = ""
            };

        }
        public async Task<Response> obtenerCompraCliente(long id)
        {

            if (id == 0)
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "Parametros incorrectos",
                    respuesta = ""
                };
            }

            DataTable dtInfo = articuloCliente.obtenerCompraCliente(id);

            if (dtInfo.Rows.Count == 0)
            {
                return new Response
                {
                    codigo = "404",
                    mensaje = "No se encontro información",
                    respuesta = ""
                };
            }

            return new Response
            {
                codigo = "200",
                mensaje = "",
                respuesta = new { data = JsonConvert.SerializeObject(dtInfo) }
            };

        }
        public async Task<Response> lista(int iTake, int iSkip)
        {

            iTake = iTake == 0 ? 99999 : iTake;
            var dtInfo = articuloCliente.lista(iTake, iSkip,context.getID());
            int totalPages = dtInfo.filas == 0 ? 0 : (int)Math.Ceiling((double)dtInfo.filas / iTake);
            return new Response
            {
                codigo = "200",
                mensaje = "",
                respuesta = new
                {
                    data = JsonConvert.SerializeObject(dtInfo),
                    totalRows = dtInfo.filas,
                    totalPages = totalPages
                }

            };
        }
    }
}
