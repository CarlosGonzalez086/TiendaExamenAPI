namespace TiendaExamenAPI.DbData.DtoModels.articulos
{
    public class dtoArticulos
    {
        public long id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public string imagen { get; set; }
        public int stock { get; set; }

    }
}
