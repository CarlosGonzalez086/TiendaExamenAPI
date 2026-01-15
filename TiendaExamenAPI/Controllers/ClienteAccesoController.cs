using Microsoft.AspNetCore.Mvc;
using TiendaExamenAPI.DbData.DtoModels.Cliente;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.Services.Cliente;

namespace TiendaExamenAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteAccesoController : ControllerBase
    {
        private readonly ClienteAccesoServicio _clienteAccesoServicio;

        public ClienteAccesoController(ClienteAccesoServicio clienteAccesoServicio)
        {
            _clienteAccesoServicio = clienteAccesoServicio;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] dtoClienteAcceso data)
        {
            Response resp = await _clienteAccesoServicio.LoginAsync(data);
            return Ok(resp);
        }
    }
}
