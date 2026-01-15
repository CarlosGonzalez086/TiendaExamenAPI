using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaExamenAPI.Modelos
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("nombre")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Column("apellidos")]
        [StringLength(150)]
        public string Apellidos { get; set; } = string.Empty;

        [Required]
        [Column("direccion")]
        public string Direccion { get; set; } = string.Empty;

        [Column("fecha")]
        public DateTime? Fecha { get; set; }

        [Column("correo_electronico")]
        [StringLength(256)]
        public string? CorreoElectronico { get; set; }

        [Column("contrasena")]
        public string? Contrasena { get; set; }

        [Column("fecha_actualizacion")]
        public DateTime? FechaActualizacion { get; set; }

        [Column("fecha_eliminado")]
        public DateTime? FechaEliminado { get; set; }

        [Required]
        [Column("eliminado")]
        public bool Eliminado { get; set; }

        [Column("token")]
        public Guid? Token { get; set; }
    }
}
