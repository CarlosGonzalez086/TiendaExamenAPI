namespace TiendaExamenAPI.DbData.DtoModels
{
    public class AdminLoginDto
    {
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
    }

    public class AdminLoginResponseDto
    {
        public long Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Apellidos { get; set; }
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Rol { get; set; } = "ADMIN";
        public string Token { get; set; } = string.Empty;
    }
}
