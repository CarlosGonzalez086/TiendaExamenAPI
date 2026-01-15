using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaExamenAPI.Modelos
{
    [Table("tiendas")]
    public class Tienda
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("sucursal")]
        [StringLength(100)]
        public string Sucursal { get; set; } = string.Empty;

        [Required]
        [Column("direccion")]
        public string Direccion { get; set; } = string.Empty;

        [Column("fecha")]
        public DateTime? Fecha { get; set; }

        [Column("fecha_actualizacion")]
        public DateTime? FechaActualizacion { get; set; }

        [Column("fecha_eliminado")]
        public DateTime? FechaEliminado { get; set; }

        [Required]
        [Column("eliminado")]
        public bool Eliminado { get; set; }
    }
}
