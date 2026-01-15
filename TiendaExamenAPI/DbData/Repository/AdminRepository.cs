using System;
using System.Collections.Generic;
using System.Linq;
using TiendaExamenAPI.Modelos;

namespace TiendaExamenAPI.DbData.Repository
{
    public class AdminRepository
    {
        private readonly MiTiendaDbContext _context;

        public AdminRepository(MiTiendaDbContext context)
        {
            _context = context;
        }

        public Admin Crear(Admin admin)
        {
            _context.Admin.Add(admin);
            _context.SaveChanges();
            return admin;
        }

        public bool Eliminar(long id)
        {
            var admin = _context.Admin.Find(id);
            if (admin == null) return false;

            admin.Eliminado = true;
            admin.FechaEliminado = DateTime.UtcNow;

            _context.Admin.Update(admin);
            _context.SaveChanges();
            return true;
        }

        public List<Admin> ObtenerTodos()
        {
            return _context.Admin
                .Where(a => !a.Eliminado)
                .ToList();
        }

        public Admin? ObtenerPorCorreo(string correo)
        {
            return _context.Admin
                .FirstOrDefault(a => a.CorreoElectronico == correo && !a.Eliminado);
        }

        public Admin? ObtenerPorId(long id)
        {
            return _context.Admin
                .FirstOrDefault(a => a.Id == id && !a.Eliminado);
        }

        public Admin? Actualizar(Admin admin)
        {
            var existente = _context.Admin.Find(admin.Id);
            if (existente == null) return null;

            existente.Nombre = admin.Nombre;
            existente.Apellidos = admin.Apellidos;
            existente.CorreoElectronico = admin.CorreoElectronico;
            if (!string.IsNullOrEmpty(admin.Contrasena))
                existente.Contrasena = admin.Contrasena;
            existente.Rol = admin.Rol;
            existente.Activo = admin.Activo;
            existente.FechaActualizacion = DateTime.UtcNow;

            _context.Admin.Update(existente);
            _context.SaveChanges();
            return existente;
        }
    }
}
