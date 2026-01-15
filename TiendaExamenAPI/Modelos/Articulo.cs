using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaExamenAPI.Modelos
{
    [Table("articulos")]
    public class Articulo
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("codigo")]
        [StringLength(50)]
        public string? Codigo { get; set; }

        [Required]
        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Column("precio", TypeName = "numeric(12,2)")]
        public decimal Precio { get; set; }

        [Column("imagen")]
        [StringLength(500)]
        public string? Imagen { get; set; }

        [Column("stock")]
        public int? Stock { get; set; }

        [Column("fecha_actualizacion")]
        public DateTime? FechaActualizacion { get; set; }

        [Column("fecha_eliminado")]
        public DateTime? FechaEliminado { get; set; }

        [Required]
        [Column("eliminado")]
        public bool Eliminado { get; set; }

        [Required]
        [Column("activo")]
        public bool Activo { get; set; }

        [Column("fecha")]
        public DateTime? Fecha { get; set; }
    }
}
