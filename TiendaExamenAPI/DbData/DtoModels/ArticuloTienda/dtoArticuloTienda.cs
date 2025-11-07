namespace TiendaExamenAPI.DbData.DtoModels.ArticuloTienda
{
    public class dtoArticuloTienda 
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public List<relacionArticuloTienda> articulos { get; set; } = new List<relacionArticuloTienda>();
    }
    public class relacionArticuloTienda
    {
        public long id { get; set; }
        public long id_tienda { get; set; }
        public long id_articulo { get; set; }
    }
}
