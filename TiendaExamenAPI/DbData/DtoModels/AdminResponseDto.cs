namespace TiendaExamenAPI.DbData.DtoModels
{
    public class AdminResponseDto
    {
        public long Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Apellidos { get; set; }
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Rol { get; set; } = "ADMIN";
        public bool Activo { get; set; }
    }
}
