using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaExamenAPI.Modelos
{
    [Table("cliente_articulo")]
    public class ClienteArticulo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("total", TypeName = "numeric(12,2)")]
        public decimal Total { get; set; }

        [Required]
        [Column("fecha", TypeName = "date")]
        public DateTime Fecha { get; set; }
    }
}
