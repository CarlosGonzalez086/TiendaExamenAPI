namespace TiendaExamenAPI.DbData.DtoModels
{
    public class AdminCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Apellidos { get; set; }
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Rol { get; set; } = "ADMIN";
        public bool Activo { get; set; } = true;
    }
}
