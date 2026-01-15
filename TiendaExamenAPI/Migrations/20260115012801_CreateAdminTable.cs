using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TiendaExamenAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateAdminTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    apellidos = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    correo_electronico = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    contrasena = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    rol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "ADMIN"),
                    token = table.Column<string>(type: "text", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_eliminado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_admin_correo_electronico",
                table: "admin",
                column: "correo_electronico",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin");
        }
    }
}
