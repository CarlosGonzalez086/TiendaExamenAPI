using System.Data;
using TiendaExamenAPI.DbData.Connection;
using TiendaExamenAPI.DbData.DtoModels.Cliente;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.DbData.Repository.Cliente;
using TiendaExamenAPI.Services.FuncionesGenerales;
using TiendaExamenAPI.Services.Utils;

namespace TiendaExamenAPI.Services.Cliente
{
    public class ClienteServicio
    {
        ClienteRepositorio cliente = new();
        ServiciosGenerales generales = new();
        Context context = new();
        public async Task<Response> guardarCliente(dtoCliente clienteInfo) 
        {
            if (string.IsNullOrEmpty(clienteInfo.nombre))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de nombre es requerido",
                    respuesta = ""
                };
            }
            if (string.IsNullOrEmpty(clienteInfo.direccion))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de la direccion es requerido",
                    respuesta = ""
                };
            }
            if (string.IsNullOrEmpty(clienteInfo.correo_electronico))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo del correo es requerido",
                    respuesta = ""
                };
            }
            if (string.IsNullOrEmpty(clienteInfo.contrasena))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de la contrasena es requerido",
                    respuesta = ""
                };
            }
            DataTable getCorreo = cliente.obtenerCorreo(clienteInfo.correo_electronico, 0);
            if (getCorreo.Rows.Count > 0)
            {
                int correos = int.Parse(getCorreo.Rows[0]["correo"].ToString());

                if (correos != 0)
                {
                    return new Response
                    {
                        codigo = "902",
                        mensaje = "Este correo ya se encuentra registrado, intenta con uno diferente o reestablece tu contraseña."
                    };
                }
            }
            clienteInfo.contrasena = generales.Encrypt(clienteInfo.contrasena);

            var insertOk = cliente.insertado(clienteInfo);

            if (insertOk.success)
            {
                return new Response
                {
                    codigo = "200",
                    mensaje = "Usuario registrado correctamente",
                };
            }
            return new Response
            {
                codigo = "900",
                mensaje = "Ha ocurrido un error al registrarse, intente más tarde",
            };

        }

        public async Task<Response> actualizarCliente(dtoCliente clienteInfo)
        {

            long userId = clienteInfo.id;

            if (userId == 0)
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "No se pudo obtener información de la sesión",
                    respuesta = ""
                };
            }

            if (string.IsNullOrEmpty(clienteInfo.nombre))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de nombre es requerido",
                    respuesta = ""
                };
            }

            if (string.IsNullOrEmpty(clienteInfo.apellidos))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de apellido paterno es requerido",
                    respuesta = ""
                };
            }

            if (string.IsNullOrEmpty(clienteInfo.direccion))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de apellido paterno es requerido",
                    respuesta = ""
                };
            }

            bool insertOk = cliente.actualizacion(clienteInfo, userId);

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
        public async Task<Response> obtenerCliente()
        {
            dtoCliente infoUser = cliente.obternerPorId(context.getID());
            infoUser.contrasena = "";

            if (infoUser != null)
            {
                return new Response
                {
                    codigo = "200",
                    mensaje = "Informacion del perfil",
                    respuesta = infoUser
                };
            }

            return new Response
            {
                codigo = "900",
                mensaje = "Ocurrio un error",
            };

        }
        public async Task<Response> EliminarCliente(long id)
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

            bool eliminado = cliente.eliminacion(id);

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
