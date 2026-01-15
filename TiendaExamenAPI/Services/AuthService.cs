using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TiendaExamenAPI.DbData.DtoModels;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.DbData.Repository;

namespace TiendaExamenAPI.Services
{
    public class AuthService
    {
        private readonly AdminRepository _repo;
        private readonly string _jwtSecret;

        public AuthService(AdminRepository repo, IConfiguration config)
        {
            _repo = repo;
            _jwtSecret = config["JwtSettings:Secret"]; 
        }

        public Response Login(AdminLoginDto dto)
        {
            var admin = _repo.ObtenerPorCorreo(dto.CorreoElectronico);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(dto.Contrasena, admin.Contrasena))
            {
                return new Response
                {
                    codigo = "401",
                    mensaje = "Correo o contraseña incorrectos",
                    respuesta = ""
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                    new Claim(ClaimTypes.Name, admin.Nombre),
                    new Claim(ClaimTypes.Email, admin.CorreoElectronico),
                    new Claim(ClaimTypes.Role, admin.Rol)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var responseDto = new AdminLoginResponseDto
            {
                Id = admin.Id,
                Nombre = admin.Nombre,
                Apellidos = admin.Apellidos,
                CorreoElectronico = admin.CorreoElectronico,
                Rol = admin.Rol,
                Token = tokenString
            };

            return new Response
            {
                codigo = "200",
                mensaje = "Login exitoso",
                respuesta = responseDto
            };
        }
    }
}
