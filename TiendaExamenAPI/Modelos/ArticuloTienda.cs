using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaExamenAPI.Modelos
{
    [Table("articulo_tienda")]
    public class ArticuloTienda
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Column("fecha")]
        public DateTime? Fecha { get; set; }

        [Column("fecha_actualizacion")]
        public DateTime? FechaActualizacion { get; set; }
    }
}
