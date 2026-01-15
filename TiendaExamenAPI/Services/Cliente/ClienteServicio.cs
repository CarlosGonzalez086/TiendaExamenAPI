using System.Data;
using TiendaExamenAPI.DbData.DtoModels.Cliente;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.DbData.Repository.Cliente;
using TiendaExamenAPI.Services.FuncionesGenerales;
using TiendaExamenAPI.Services.Utils;

namespace TiendaExamenAPI.Services.Cliente
{
    public class ClienteServicio
    {
        private readonly ClienteRepositorio _clienteRepositorio;
        private readonly ServiciosGenerales _generales;
        private readonly Context _context;

        public ClienteServicio(
            ClienteRepositorio clienteRepositorio,
            ServiciosGenerales generales,
            Context context)
        {
            _clienteRepositorio = clienteRepositorio;
            _generales = generales;
            _context = context;
        }
        public async Task<Response> GuardarClienteAsync(dtoCliente clienteInfo)
        {
            if (string.IsNullOrWhiteSpace(clienteInfo.nombre))
                return await Task.FromResult(ResponseError("901", "El campo nombre es requerido"));

            if (string.IsNullOrWhiteSpace(clienteInfo.direccion))
                return await Task.FromResult(ResponseError("901", "El campo dirección es requerido"));

            if (string.IsNullOrWhiteSpace(clienteInfo.correo_electronico))
                return await Task.FromResult(ResponseError("901", "El campo correo es requerido"));

            if (string.IsNullOrWhiteSpace(clienteInfo.contrasena))
                return await Task.FromResult(ResponseError("901", "El campo contraseña es requerido"));

            var correoExiste = await _clienteRepositorio.ExisteCorreoAsync(
                clienteInfo.correo_electronico, 0);

            if (correoExiste)
            {
                return await Task.FromResult(new Response
                {
                    codigo = "902",
                    mensaje = "Este correo ya se encuentra registrado"
                });
            }

            clienteInfo.contrasena = _generales.Encrypt(clienteInfo.contrasena);

            var result = _clienteRepositorio.InsertadoAsync(clienteInfo);

            if (!result.IsCompleted)
            {
                return await Task.FromResult(new Response
                {
                    codigo = "900",
                    mensaje = "Error al registrar usuario"
                });
            }

            return await Task.FromResult(new Response
            {
                codigo = "200",
                mensaje = "Usuario registrado correctamente"
            });
        }

        public async Task<Response> ActualizarClienteAsync(dtoCliente clienteInfo)
        {
            long userId = clienteInfo.id;

            if (userId == 0)
                return await Task.FromResult(ResponseError("901", "Sesión inválida"));

            if (string.IsNullOrWhiteSpace(clienteInfo.nombre))
                return await Task.FromResult(ResponseError("901", "El nombre es requerido"));

            if (string.IsNullOrWhiteSpace(clienteInfo.apellidos))
                return await Task.FromResult(ResponseError("901", "Los apellidos son requeridos"));

            if (string.IsNullOrWhiteSpace(clienteInfo.direccion))
                return await Task.FromResult(ResponseError("901", "La dirección es requerida"));

            bool actualizado = await _clienteRepositorio.ActualizacionAsync(clienteInfo, userId);

            if (!actualizado)
            {
                return await Task.FromResult(new Response
                {
                    codigo = "900",
                    mensaje = "Error al actualizar"
                });
            }

            return await Task.FromResult(new Response
            {
                codigo = "200",
                mensaje = "Actualizado correctamente"
            });
        }

        public async Task<Response> ObtenerClienteAsync()
        {
            long userId = _context.getID();

            if (userId == 0)
                return await Task.FromResult(ResponseError("901", "Sesión inválida"));

            dtoCliente cliente = await _clienteRepositorio.ObtenerPorIdAsync(userId);

            if (cliente == null)
            {
                return await Task.FromResult(new Response
                {
                    codigo = "404",
                    mensaje = "Cliente no encontrado"
                });
            }

            cliente.contrasena = string.Empty;

            return await Task.FromResult(new Response
            {
                codigo = "200",
                mensaje = "Información del perfil",
                respuesta = cliente
            });
        }


        public async Task<Response> EliminarClienteAsync(long id)
        {
            if (id == 0)
                return await Task.FromResult(ResponseError("901", "Parámetros incorrectos"));

            bool eliminado = await _clienteRepositorio.EliminacionAsync(id);

            if (!eliminado)
            {
                return await Task.FromResult(new Response
                {
                    codigo = "900",
                    mensaje = "Error al eliminar"
                });
            }

            return await Task.FromResult(new Response
            {
                codigo = "200",
                mensaje = "Eliminado correctamente"
            });
        }
        private static Response ResponseError(string codigo, string mensaje) =>
            new()
            {
                codigo = codigo,
                mensaje = mensaje,
                respuesta = ""
            };
    }
}
