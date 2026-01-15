using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TiendaExamenAPI.DbData
{
    public class MiTiendaDbContextFactory
           : IDesignTimeDbContextFactory<MiTiendaDbContext>
    {
        public MiTiendaDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<MiTiendaDbContext>();

            optionsBuilder.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")
            );

            return new MiTiendaDbContext(optionsBuilder.Options);
        }
    }
}
