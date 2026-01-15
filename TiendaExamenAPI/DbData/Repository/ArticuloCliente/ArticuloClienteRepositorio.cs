using Microsoft.EntityFrameworkCore;
using TiendaExamenAPI.Modelos;

namespace TiendaExamenAPI.DbData.Repository.ArticuloCliente
{
    public class ArticuloClienteRepositorio
    {
        private readonly MiTiendaDbContext _context;

        public ArticuloClienteRepositorio(MiTiendaDbContext context)
        {
            _context = context;
        }
        public async Task<(bool success, long insertedId)> InsertadoAsync(decimal total)
        {
            try
            {
                var entity = new ClienteArticulo
                {
                    Total = total,
                    Fecha = DateTime.UtcNow
                };

                _context.ClienteArticulos.Add(entity);
                await _context.SaveChangesAsync();

                return (true, entity.Id);
            }
            catch
            {
                return (false, 0);
            }
        }
        public async Task<bool> InsertadoDetalleAsync(
            int idClienteArticulo,
            int articuloId,
            long clienteId,
            int cantidad)
        {
            try
            {
                var detalle = new RelClienteArticulo
                {
                    IdClienteArticulo = idClienteArticulo,
                    ArticuloId = articuloId,
                    ClienteId = clienteId,
                    Cantidad = cantidad,
                    Fecha = DateTime.UtcNow
                };

                _context.RelClienteArticulos.Add(detalle);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<ClienteArticulo?> ObtenerCompraClienteAsync(long id)
        {
            return await _context.ClienteArticulos
                .FirstOrDefaultAsync(ca => ca.Id == id);
        }


        public async Task<(List<ClienteArticulo> data, int total)> ListaAsync(
            int take,
            int skip,
            long clienteId)
        {
            var query = _context.ClienteArticulos
                .Where(ca =>
                    _context.RelClienteArticulos
                        .Any(rca =>
                            rca.IdClienteArticulo == ca.Id &&
                            rca.ClienteId == clienteId))
                .OrderByDescending(ca => ca.Fecha);

            var total = await query.CountAsync();

            var data = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (data, total);
        }
    }
}
