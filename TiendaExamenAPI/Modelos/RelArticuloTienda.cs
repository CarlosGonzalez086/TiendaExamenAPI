using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaExamenAPI.Modelos
{
    [Table("rel_articulo_tienda")]
    public class RelArticuloTienda
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("articulo_id")]
        public long ArticuloId { get; set; }

        [Required]
        [Column("tienda_id")]
        public long TiendaId { get; set; }

        [Required]
        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Required]
        [Column("id_articulo_tienda")]
        public long IdArticuloTienda { get; set; }
    }
}
