using Microsoft.AspNetCore.Mvc;
using TiendaExamenAPI.DbData.DtoModels;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.Services;

namespace TiendaExamenAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public ActionResult<Response> Login([FromBody] AdminLoginDto dto)
        {
            var response = _service.Login(dto);
            return StatusCode(int.Parse(response.codigo), response);
        }
    }
}
