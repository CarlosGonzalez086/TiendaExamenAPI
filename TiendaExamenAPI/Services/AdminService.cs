using TiendaExamenAPI.DbData.DtoModels;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.DbData.Repository;
using TiendaExamenAPI.Modelos;


namespace TiendaExamenAPI.Services
{
    public class AdminService
    {
        private readonly AdminRepository _repo;

        public AdminService(AdminRepository repo)
        {
            _repo = repo;
        }

        public Response CrearAdmin(AdminCreateDto dto)
        {
            var existente = _repo.ObtenerPorCorreo(dto.CorreoElectronico);
            if (existente != null)
                return new Response { codigo = "409", mensaje = "Correo electrónico ya registrado", respuesta = "" };

            var admin = new Admin
            {
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                CorreoElectronico = dto.CorreoElectronico,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                Rol = dto.Rol,
                Activo = dto.Activo
            };

            var creado = _repo.Crear(admin);

            var responseDto = new AdminResponseDto
            {
                Id = creado.Id,
                Nombre = creado.Nombre,
                Apellidos = creado.Apellidos,
                CorreoElectronico = creado.CorreoElectronico,
                Rol = creado.Rol,
                Activo = creado.Activo
            };

            return new Response { codigo = "201", mensaje = "Admin creado correctamente", respuesta = responseDto };
        }

        public Response ObtenerAdminPorId(long id)
        {
            var admin = _repo.ObtenerPorId(id);
            if (admin == null)
                return new Response { codigo = "404", mensaje = "Admin no encontrado", respuesta = "" };

            var responseDto = new AdminResponseDto
            {
                Id = admin.Id,
                Nombre = admin.Nombre,
                Apellidos = admin.Apellidos,
                CorreoElectronico = admin.CorreoElectronico,
                Rol = admin.Rol,
                Activo = admin.Activo
            };

            return new Response { codigo = "200", mensaje = "OK", respuesta = responseDto };
        }

        public Response ObtenerTodos()
        {
            var admins = _repo.ObtenerTodos();
            var responseDtos = admins.Select(a => new AdminResponseDto
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Apellidos = a.Apellidos,
                CorreoElectronico = a.CorreoElectronico,
                Rol = a.Rol,
                Activo = a.Activo
            }).ToList();

            return new Response { codigo = "200", mensaje = "OK", respuesta = responseDtos };
        }

        public Response ActualizarAdmin(long id, AdminUpdateDto dto)
        {
            var admin = _repo.ObtenerPorId(id);
            if (admin == null)
                return new Response { codigo = "404", mensaje = "Admin no encontrado", respuesta = "" };

            admin.Nombre = dto.Nombre;
            admin.Apellidos = dto.Apellidos;
            admin.CorreoElectronico = dto.CorreoElectronico;
            if (!string.IsNullOrEmpty(dto.Contrasena))
                admin.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);
            admin.Rol = dto.Rol;
            admin.Activo = dto.Activo;

            var actualizado = _repo.Actualizar(admin);

            var responseDto = new AdminResponseDto
            {
                Id = actualizado!.Id,
                Nombre = actualizado.Nombre,
                Apellidos = actualizado.Apellidos,
                CorreoElectronico = actualizado.CorreoElectronico,
                Rol = actualizado.Rol,
                Activo = actualizado.Activo
            };

            return new Response { codigo = "200", mensaje = "Admin actualizado correctamente", respuesta = responseDto };
        }

        public Response EliminarAdmin(long id)
        {
            var eliminado = _repo.Eliminar(id);
            if (!eliminado)
                return new Response { codigo = "404", mensaje = "Admin no encontrado", respuesta = "" };

            return new Response { codigo = "200", mensaje = "Admin eliminado correctamente", respuesta = "" };
        }
    }
}
