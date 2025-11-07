using System.Data;
using TiendaExamenAPI.DbData.DtoModels.Cliente;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.DbData.DtoModels.Tienda;
using TiendaExamenAPI.DbData.Repository.Tienda;

namespace TiendaExamenAPI.Services.Tienda
{
    public class TiendaServicios
    {
        TiendaRepositorio tienda = new();

        public async Task<Response> guardarTienda(dtoTienda tiendaInfo)
        {
            if (string.IsNullOrEmpty(tiendaInfo.sucursal))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de la sucursal es requerido",
                    respuesta = ""
                };
            }
            if (string.IsNullOrEmpty(tiendaInfo.direccion))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de la direccion es requerido",
                    respuesta = ""
                };
            }

            var insertOk = await tienda.insertado(tiendaInfo);

            if (insertOk)
            {
                return new Response
                {
                    codigo = "200",
                    mensaje = "Tienda registrada correctamente",
                };
            }
            return new Response
            {
                codigo = "900",
                mensaje = "Ha ocurrido un error al registrarse, intente más tarde",
            };

        }

        public async Task<Response> actualizarTienda(dtoTienda tiendaInfo)
        {

            long userId = tiendaInfo.id;

            if (userId == 0)
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "No se pudo obtener información de la sesión",
                    respuesta = ""
                };
            }

            if (string.IsNullOrEmpty(tiendaInfo.sucursal))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de la sucursal es requerido",
                    respuesta = ""
                };
            }

            if (string.IsNullOrEmpty(tiendaInfo.direccion))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de apellido de la direccion es requerido",
                    respuesta = ""
                };
            }


            bool insertOk = tienda.actualizacion(tiendaInfo, userId);

            if (!insertOk)
            {
                return new Response
                {
                    codigo = "900",
                    mensaje = "Ha ocurrido un error al actualizar, intente más tarde",
                };
            }

            return new Response
            {
                codigo = "200",
                mensaje = "Guardado correctamente",
                respuesta = ""
            };

        }
        public async Task<Response> obtenerTienda(long id)
        {

            dtoTienda infoTienda = tienda.obternerPorId(id);


            if (infoTienda != null)
            {
                return new Response
                {
                    codigo = "200",
                    mensaje = "Informacion de la tienda",
                    respuesta = infoTienda
                };
            }

            return new Response
            {
                codigo = "900",
                mensaje = "Ocurrio un error",
            };

        }
        public async Task<Response> EliminarTienda(long id)
        {
            if (id == null || id == 0)
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "Parametros incorrectos",
                    respuesta = ""
                };
            }

            bool eliminado = tienda.eliminacion(id);

            if (!eliminado)
            {
                return new Response
                {
                    codigo = "900",
                    mensaje = "Ha ocurrido un error al eliminar, intente más tarde",
                    respuesta = ""
                };
            }
            return new Response
            {
                codigo = "200",
                mensaje = "Eliminado correctamente",
                respuesta = ""
            };
        }
    }
}
