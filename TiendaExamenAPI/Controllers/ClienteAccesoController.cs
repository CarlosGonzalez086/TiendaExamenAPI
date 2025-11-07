using Microsoft.AspNetCore.Mvc;
using TiendaExamenAPI.DbData.DtoModels.Cliente;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.Services.Cliente;

namespace TiendaExamenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteAccesoController : ControllerBase
    {
        ClienteAccesoServicio clienteAcceso = new();
        [HttpPost]
        public async Task<IActionResult> login(dtoClienteAcceso data)
        {
            await Task.Delay(1000);
            Response resp = new();

            resp = await clienteAcceso.login(data);
            return Ok(resp);
        }
    }
}
