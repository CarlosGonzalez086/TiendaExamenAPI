using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TiendaExamenAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMiTienda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "articulos",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    precio = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    imagen = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    stock = table.Column<int>(type: "integer", nullable: true),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_eliminado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articulos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cliente_articulo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    total = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    fecha = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente_articulo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    apellidos = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    direccion = table.Column<string>(type: "text", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    correo_electronico = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    contrasena = table.Column<string>(type: "text", nullable: true),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_eliminado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    token = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rel_articulo_tienda",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    articulo_id = table.Column<long>(type: "bigint", nullable: false),
                    tienda_id = table.Column<long>(type: "bigint", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    id_articulo_tienda = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_articulo_tienda", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rel_cliente_articulo",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cliente_id = table.Column<long>(type: "bigint", nullable: false),
                    articulo_id = table.Column<long>(type: "bigint", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    id_cliente_articulo = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_cliente_articulo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tiendas",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sucursal = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    direccion = table.Column<string>(type: "text", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_eliminado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tiendas", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articulos");

            migrationBuilder.DropTable(
                name: "cliente_articulo");

            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "rel_articulo_tienda");

            migrationBuilder.DropTable(
                name: "rel_cliente_articulo");

            migrationBuilder.DropTable(
                name: "tiendas");
        }
    }
}
