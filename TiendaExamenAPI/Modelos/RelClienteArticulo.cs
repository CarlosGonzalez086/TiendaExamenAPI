using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaExamenAPI.Modelos
{
    [Table("rel_cliente_articulo")]
    public class RelClienteArticulo
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("cliente_id")]
        public long ClienteId { get; set; }

        [Required]
        [Column("articulo_id")]
        public long ArticuloId { get; set; }

        [Column("fecha")]
        public DateTime? Fecha { get; set; }

        [Required]
        [Column("id_cliente_articulo")]
        public int IdClienteArticulo { get; set; }

        [Required]
        [Column("cantidad")]
        public int Cantidad { get; set; }
    }
}
