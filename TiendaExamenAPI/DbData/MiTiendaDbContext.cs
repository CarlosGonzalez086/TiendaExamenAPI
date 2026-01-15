

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TiendaExamenAPI.Modelos;
using TiendaExamenAPI.Services.Utils;


namespace TiendaExamenAPI.DbData
{
    public class MiTiendaDbContext : DbContext
    {
        public MiTiendaDbContext(DbContextOptions<MiTiendaDbContext> options)
            : base(options) { }

        public DbSet<Articulo> Articulos => Set<Articulo>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Tienda> Tiendas => Set<Tienda>();
        public DbSet<ClienteArticulo> ClienteArticulos => Set<ClienteArticulo>();
        public DbSet<RelArticuloTienda> RelArticuloTiendas => Set<RelArticuloTienda>();
        public DbSet<RelClienteArticulo> RelClienteArticulos => Set<RelClienteArticulo>();
        public DbSet<Admin> Admin => Set<Admin>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.CorreoElectronico)
                      .IsUnique();

                entity.Property(e => e.Rol)
                      .HasDefaultValue("ADMIN");

                entity.Property(e => e.Activo)
                      .HasDefaultValue(true);

                entity.Property(e => e.Eliminado)
                      .HasDefaultValue(false);

                entity.Property(e => e.FechaCreacion)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
