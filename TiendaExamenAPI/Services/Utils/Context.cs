using System.Security.Claims;
using static TiendaExamenAPI.Services.Keys;

namespace TiendaExamenAPI.Services.Utils
{
    public class Context
    {
        public HttpContext httpContext;
        public Context()
        {
            httpContext = ProviderContext.GetContext();
        }

        public long getID()
        {
            long id = 0;
            try
            {
                var identity = httpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    List<Claim> claims = identity.Claims.ToList();
                    var dato = identity.FindFirst(JWTKEYS.KEY_ID);
                    if (dato != null)
                    {
                        id = Convert.ToInt64(dato.Value);
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;

        }
        public string getPass()
        {
            string pass = "";
            try
            {
                var identity = httpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var dato = identity.FindFirst(JWTKEYS.KEY_PASSWORD);
                    if (dato != null)
                    {
                        pass = dato.Value.ToString();
                    }
                }
            }
            catch (Exception)
            {
            }
            return pass;

        }
        public string getCreacion()
        {
            string creacion = "";
            try
            {
                var identity = httpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var dato = identity.FindFirst(JWTKEYS.KEY_CREACION);
                    if (dato != null)
                    {
                        creacion = dato.Value.ToString();
                    }
                }
            }
            catch (Exception)
            {

            }
            return creacion;
        }

        public string getTipoUsuario()
        {
            string role = string.Empty;
            try
            {
                var identity = httpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {

                    var roleClaim = identity.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                    if (roleClaim != null)
                    {
                        role = roleClaim.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

            return role;
        }
    }
}
