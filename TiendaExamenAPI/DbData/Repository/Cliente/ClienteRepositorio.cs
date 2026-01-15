using Microsoft.EntityFrameworkCore;
using TiendaExamenAPI.DbData.DtoModels.Cliente;



namespace TiendaExamenAPI.DbData.Repository.Cliente
{
    public class ClienteRepositorio
    {
        private readonly MiTiendaDbContext _context;

        public ClienteRepositorio(MiTiendaDbContext context)
        {
            _context = context;
        }
        public async Task<dtoCliente?> ObtenerPorIdAsync(long id)
        {
            return await _context.Clientes
                .Where(c => c.Id == id && !c.Eliminado)
                .Select(c => new dtoCliente
                {
                    id = c.Id,
                    nombre = c.Nombre,
                    apellidos = c.Apellidos,
                    direccion = c.Direccion,
                    correo_electronico = c.CorreoElectronico,
                    contrasena = c.Contrasena
                })
                .FirstOrDefaultAsync();
        }
        public async Task<bool> ExisteCorreoAsync(string email, long id = 0)
        {
            return await _context.Clientes.AnyAsync(c =>
                c.CorreoElectronico == email &&
                !c.Eliminado &&
                c.Id != id);
        }

        public async Task<dtoCliente?> ObtenerInfoLoginAsync(
            string correoElectronico,
            string contrasena)
        {
            return await _context.Clientes
                .Where(c =>
                    c.CorreoElectronico == correoElectronico &&
                    c.Contrasena == contrasena &&
                    !c.Eliminado)
                .Select(c => new dtoCliente
                {
                    id = c.Id,
                    nombre = c.Nombre,
                    apellidos = c.Apellidos,
                    direccion = c.Direccion,
                    correo_electronico = c.CorreoElectronico,
                    contrasena = c.Contrasena
                })
                .FirstOrDefaultAsync();
        }

        public async Task<(bool success, long insertedId)> InsertadoAsync(dtoCliente dto)
        {
            try
            {
                var cliente = new TiendaExamenAPI.Modelos.Cliente
                {
                    Nombre = dto.nombre,
                    Apellidos = dto.apellidos,
                    Direccion = dto.direccion,
                    CorreoElectronico = dto.correo_electronico,
                    Contrasena = dto.contrasena,
                    Fecha = DateTime.UtcNow,
                    Eliminado = false
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                return (true, cliente.Id);
            }
            catch
            {
                return (false, 0);
            }
        }

        public async Task<bool> ActualizacionAsync(dtoCliente dto, long id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            cliente.Nombre = dto.nombre;
            cliente.Apellidos = dto.apellidos;
            cliente.Direccion = dto.direccion;
            cliente.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminacionAsync(long id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            cliente.Eliminado = true;
            cliente.FechaEliminado = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarTokenAsync(long id, Guid token)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            cliente.Token = token;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
