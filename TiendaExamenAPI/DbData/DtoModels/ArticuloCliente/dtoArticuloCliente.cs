namespace TiendaExamenAPI.DbData.DtoModels.ArticuloCliente
{
    public class dtoArticuloCliente
    {
        public int id { get; set; }
        public decimal total { get; set; }
        public int cliente_id { get; set; }
        public List<dtoArticuloClienteDetalle> articulos { get; set; } = new List<dtoArticuloClienteDetalle>();

    }

    public class dtoArticuloClienteDetalle
    {
        public int id { get; set; }
        public int articulo_id { get; set; }
        public int cantidad { get; set; }
        public int id_cliente_articulo { get; set; }
    }
}
