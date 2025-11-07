using Microsoft.IdentityModel.Tokens;
using System.Data;
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
        ServiciosGenerales generales = new();
        ClienteRepositorio cliente = new();

        public async Task<Response> login(dtoClienteAcceso data)
        {
            long ID = 0;
            string NOMBRE = "";
            string CORREO = "";
            string PASSWORD = "";
            string IMG = "";
            if (string.IsNullOrEmpty(data.correo_electronico))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de correo no puede estar vacío",
                    respuesta = ""
                };
            }

            if (string.IsNullOrEmpty(data.contrasena))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de contraseña no puede estar vacio",
                    respuesta = ""
                };
            }

            data.contrasena = generales.Encrypt(data.contrasena);
            DataTable dtInfo = cliente.obtenerInfoLogin(data.correo_electronico, data.contrasena);
            if (dtInfo.Rows.Count > 0)
            {
                ID = Convert.ToInt64(dtInfo.Rows[0]["ID"].ToString());
                NOMBRE = dtInfo.Rows[0]["nombre_completo"].ToString();
                CORREO = dtInfo.Rows[0]["correo_electronico"].ToString();
                PASSWORD = dtInfo.Rows[0]["contrasena"].ToString();
                string jwt = generateJWT(ID.ToString(), CORREO, PASSWORD, DateTime.Now);
                if (string.IsNullOrEmpty(jwt))
                {
                    return new Response
                    {
                        codigo = "902",
                        mensaje = "Ocurrio un error a el generar un token de autenticación.",
                        respuesta = ""
                    };
                }
                return new Response
                {
                    codigo = "200",
                    mensaje = "Acceso correcto",
                    respuesta = new
                    {
                        token = jwt,

                    }
                };
            }
            return new Response
            {
                codigo = "403",
                mensaje = "Acceso incorrecto",
                respuesta = ""
            };
        }
        public string generateJWT(string id, string correo, string password, DateTime creacion)
        {
            Clave data = new Clave();
            string clave = data.getClave();
            try
            {
                var claims = new[]
                {
                    new Claim(JWTKEYS.KEY_ID, id),
                    new Claim(JWTKEYS.KEY_CORREO, correo),
                    new Claim(JWTKEYS.KEY_PASSWORD, password),
                    new Claim(JWTKEYS.KEY_CREACION, creacion.ToString()),
                    new Claim(ClaimTypes.Role, JWTKEYS.KEY_ROL_CLIENTE),
                };

                DateTime d = DateTime.Now.AddDays(30);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                var securytyToken = new JwtSecurityToken(
                    claims: claims,
                    expires: d,
                    signingCredentials: creds
                    );

                string Token = new JwtSecurityTokenHandler().WriteToken(securytyToken);
                return Token;
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
