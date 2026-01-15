using Microsoft.EntityFrameworkCore;
using TiendaExamenAPI.DbData;
using TiendaExamenAPI.DbData.DtoModels.articulos;
using TiendaExamenAPI.Modelos;

namespace TiendaExamenAPI.DbData.Repository.Articulos
{
    public class ArticulosRepositorio
    {
        private readonly MiTiendaDbContext _context;

        public ArticulosRepositorio(MiTiendaDbContext context)
        {
            _context = context;
        }
        public async Task<dtoArticulos?> ObtenerPorIdAsync(long id)
        {
            return await _context.Articulos
                .Where(a => a.Id == id && !a.Eliminado)
                .Select(a => new dtoArticulos
                {
                    id = a.Id,
                    codigo = a.Codigo,
                    descripcion = a.Descripcion,
                    precio = a.Precio,
                    imagen = a.Imagen,
                    stock = a.Stock ?? 0
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> InsertadoAsync(dtoArticulos dto)
        {
            try
            {
                var entity = new Articulo
                {
                    Codigo = dto.codigo,
                    Descripcion = dto.descripcion,
                    Imagen = dto.imagen,
                    Precio = dto.precio,
                    Stock = dto.stock,
                    Fecha = DateTime.UtcNow,
                    Eliminado = false,
                    Activo = true
                };

                _context.Articulos.Add(entity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActualizacionAsync(dtoArticulos dto, long id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null) return false;

            articulo.Codigo = dto.codigo;
            articulo.Descripcion = dto.descripcion;
            articulo.Imagen = dto.imagen;
            articulo.Stock = dto.stock;
            articulo.Precio = dto.precio;
            articulo.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminacionAsync(long id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null) return false;

            articulo.Eliminado = true;
            articulo.FechaEliminado = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(List<dtoArticulos> data, int total)> ListaAsync(
            int take,
            int skip)
        {
            var query = _context.Articulos
                .Where(a => !a.Eliminado)
                .OrderByDescending(a => a.Fecha);

            var total = await query.CountAsync();

            var data = await query
                .Skip(skip)
                .Take(take)
                .Select(a => new dtoArticulos
                {
                    id = a.Id,
                    codigo = a.Codigo,
                    descripcion = a.Descripcion,
                    imagen = a.Imagen,
                    stock = a.Stock ?? 0,
                    precio = a.Precio
                })
                .ToListAsync();

            return (data, total);
        }
    }
}
