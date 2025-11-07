using Azure;
using Microsoft.AspNetCore.Mvc;
using TiendaExamenAPI.DbData.DtoModels.Cliente;
using TiendaExamenAPI.Services.Cliente;

namespace TiendaExamenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        ClienteServicio cliente = new();

        [HttpPost]
        public async Task<IActionResult> guardarCliente(dtoCliente data)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await cliente.guardarCliente(data);
            return Ok(resp);
        }

        [HttpPut]
        public async Task<IActionResult> actualizarCliente(dtoCliente userInfo)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await cliente.actualizarCliente(userInfo);
            return Ok(resp);
        }

        [HttpGet]
        public async Task<IActionResult> obtenerCliente()
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await cliente.obtenerCliente();
            return Ok(resp);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCliente(long id)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await cliente.EliminarCliente(id);
            return Ok(resp);
        }

    }
}
