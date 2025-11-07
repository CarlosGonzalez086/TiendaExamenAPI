namespace TiendaExamenAPI.Services.Utils
{
    public class ProviderContext
    {
        private static HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        public static HttpContext GetContext()
        {
            return _httpContext;
        }
    }
}
