using Microsoft.AspNetCore.Mvc;
using TiendaExamenAPI.DbData.DtoModels.Cliente;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.Services.Cliente;

namespace TiendaExamenAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteServicio _clienteServicio;

        public ClienteController(ClienteServicio clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        [HttpPost]
        public async Task<IActionResult> GuardarCliente([FromBody] dtoCliente data)
        {
            Response resp = await _clienteServicio.GuardarClienteAsync(data);
            return Ok(resp);
        }


        [HttpPut]
        public async Task<IActionResult> ActualizarCliente([FromBody] dtoCliente data)
        {
            Response resp = await _clienteServicio.ActualizarClienteAsync(data);
            return Ok(resp);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCliente()
        {
            Response resp = await _clienteServicio.ObtenerClienteAsync();
            return Ok(resp);
        }


        [HttpDelete("{id:long}")]
        public async Task<IActionResult> EliminarCliente(long id)
        {
            Response resp = await _clienteServicio.EliminarClienteAsync(id);
            return Ok(resp);
        }
    }
}
