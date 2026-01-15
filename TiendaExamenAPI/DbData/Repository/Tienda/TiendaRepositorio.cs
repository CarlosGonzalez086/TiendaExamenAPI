using Microsoft.EntityFrameworkCore;
using TiendaExamenAPI.DbData;
using TiendaExamenAPI.DbData.DtoModels.Tienda;


namespace TiendaExamenAPI.DbData.Repository.Tienda
{
    public class TiendaRepositorio
    {
        private readonly MiTiendaDbContext _context;

        public TiendaRepositorio(MiTiendaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> InsertadoAsync(dtoTienda dto)
        {
            try
            {
                var tienda = new TiendaExamenAPI.Modelos.Tienda
                {
                    Sucursal = dto.sucursal,
                    Direccion = dto.direccion,
                    Fecha = DateTime.UtcNow,
                    Eliminado = false
                };

                _context.Tiendas.Add(tienda);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<dtoTienda?> ObtenerPorIdAsync(long id)
        {
            return await _context.Tiendas
                .Where(t => t.Id == id && !t.Eliminado)
                .Select(t => new dtoTienda
                {
                    id = t.Id,
                    sucursal = t.Sucursal,
                    direccion = t.Direccion
                })
                .FirstOrDefaultAsync();
        }


        public async Task<bool> ActualizacionAsync(dtoTienda dto, long id)
        {
            var tienda = await _context.Tiendas.FindAsync(id);
            if (tienda == null) return false;

            tienda.Sucursal = dto.sucursal;
            tienda.Direccion = dto.direccion;
            tienda.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> EliminacionAsync(long id)
        {
            var tienda = await _context.Tiendas.FindAsync(id);
            if (tienda == null) return false;

            tienda.Eliminado = true;
            tienda.FechaEliminado = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<dtoTienda>> ObtenerTodosAsync()
        {
            return await _context.Tiendas
                .Where(t => !t.Eliminado) 
                .Select(t => new dtoTienda
                {
                    id = t.Id,
                    sucursal = t.Sucursal,
                    direccion = t.Direccion
                })
                .ToListAsync();
        }

    }
}
