using Microsoft.AspNetCore.Mvc;
using TiendaExamenAPI.DbData.DtoModels.Tienda;
using TiendaExamenAPI.Services.Tienda;

namespace TiendaExamenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiendaController : ControllerBase
    {
        TiendaServicios tienda = new();

        [HttpPost]
        public async Task<IActionResult> guardarTienda(dtoTienda data)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await tienda.guardarTienda(data);
            return Ok(resp);
        }

        [HttpPut]
        public async Task<IActionResult> actualizarCliente(dtoTienda data)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await tienda.actualizarTienda(data);
            return Ok(resp);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> obtenerTienda(long id)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await tienda.obtenerTienda(id);
            return Ok(resp);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTienda(long id)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await tienda.EliminarTienda(id);
            return Ok(resp);
        }

    }
}
