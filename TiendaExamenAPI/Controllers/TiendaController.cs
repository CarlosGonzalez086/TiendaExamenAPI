using Microsoft.AspNetCore.Mvc;
using TiendaExamenAPI.DbData.DtoModels.Tienda;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.Services.Tienda;

namespace TiendaExamenAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TiendaController : ControllerBase
    {
        private readonly TiendaServicios _tienda;

        public TiendaController(TiendaServicios tienda)
        {
            _tienda = tienda;
        }

        [HttpPost]
        public async Task<IActionResult> GuardarTienda([FromBody] dtoTienda data)
        {
            Response resp = await _tienda.GuardarTiendaAsync(data);
            return Ok(resp);
        }


        [HttpPut]
        public async Task<IActionResult> ActualizarTienda([FromBody] dtoTienda data)
        {
            Response resp = await _tienda.ActualizarTiendaAsync(data);
            return Ok(resp);
        }


        [HttpGet("{id:long}")]
        public async Task<IActionResult> ObtenerTienda(long id)
        {
            Response resp = await _tienda.ObtenerTiendaAsync(id);
            return Ok(resp);
        }


        [HttpDelete("{id:long}")]
        public async Task<IActionResult> EliminarTienda(long id)
        {
            Response resp = await _tienda.EliminarTiendaAsync(id);
            return Ok(resp);
        }
        [HttpGet("all")]
        public async Task<IActionResult> ObtenerTiendas()
        {
            Response resp = await _tienda.ObtenerTiendasAsync();
            return Ok(resp);
        }
    }
}
