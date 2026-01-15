using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TiendaExamenAPI.DbData.DtoModels.Cliente;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.DbData.Repository.Cliente;
using TiendaExamenAPI.Services.FuncionesGenerales;
using static TiendaExamenAPI.Services.Keys;

namespace TiendaExamenAPI.Services.Cliente
{
    public class ClienteAccesoServicio
    {
        private readonly ClienteRepositorio _clienteRepositorio;
        private readonly ServiciosGenerales _generales;
        private readonly IConfiguration _config;

        public ClienteAccesoServicio(
            ClienteRepositorio clienteRepositorio,
            ServiciosGenerales generales,
            IConfiguration config)
        {
            _clienteRepositorio = clienteRepositorio;
            _generales = generales;
            _config = config;
        }

        public async Task<Response> LoginAsync(dtoClienteAcceso data)
        {
            if (string.IsNullOrWhiteSpace(data.correo_electronico))
            {
                return ResponseError("901", "El campo correo es requerido");
            }

            if (string.IsNullOrWhiteSpace(data.contrasena))
            {
                return ResponseError("901", "El campo contraseña es requerido");
            }

            string passwordHash = _generales.Encrypt(data.contrasena);

            var cliente = await _clienteRepositorio.ObtenerInfoLoginAsync(
                data.correo_electronico,
                passwordHash);

            if (cliente == null)
            {
                return new Response
                {
                    codigo = "403",
                    mensaje = "Acceso incorrecto",
                    respuesta = ""
                };
            }

            string token = GenerarJwt(cliente.id, cliente.correo_electronico);

            if (string.IsNullOrEmpty(token))
            {
                return ResponseError("902", "Error al generar token");
            }

            return new Response
            {
                codigo = "200",
                mensaje = "Acceso correcto",
                respuesta = new
                {
                    token
                }
            };
        }

        private string GenerarJwt(long id, string correo)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(JWTKEYS.KEY_ID, id.ToString()),
                    new Claim(JWTKEYS.KEY_CORREO, correo),
                    new Claim(ClaimTypes.Role, JWTKEYS.KEY_ROL_CLIENTE),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                string secretKey = _config["Jwt:Key"];
                string issuer = _config["Jwt:Issuer"];
                string audience = _config["Jwt:Audience"];

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey));

                var creds = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(30),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                return null;
            }
        }

        private static Response ResponseError(string codigo, string mensaje) =>
            new()
            {
                codigo = codigo,
                mensaje = mensaje,
                respuesta = ""
            };
    }
}
