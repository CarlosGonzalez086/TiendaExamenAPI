using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaExamenAPI.Modelos
{
    [Table("admin")]
    public class Admin
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(150)]
        [Column("apellidos")]
        public string? Apellidos { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("correo_electronico")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("contrasena")]
        public string Contrasena { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("rol")]
        public string Rol { get; set; } = "ADMIN";

        [Column("token")]
        public string? Token { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("eliminado")]
        public bool Eliminado { get; set; } = false;

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Column("fecha_actualizacion")]
        public DateTime? FechaActualizacion { get; set; }

        [Column("fecha_eliminado")]
        public DateTime? FechaEliminado { get; set; }
    }
}
