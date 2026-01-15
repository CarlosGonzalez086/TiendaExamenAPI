using Microsoft.AspNetCore.Mvc;
using TiendaExamenAPI.DbData.DtoModels.ArticuloCliente;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.Services.ArticuloCliente;

namespace TiendaExamenAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompraClienteController : ControllerBase
    {
        private readonly ArticuloClienteServicios _articuloClienteServicios;

        public CompraClienteController(ArticuloClienteServicios articuloClienteServicios)
        {
            _articuloClienteServicios = articuloClienteServicios;
        }


        [HttpPost]
        public async Task<IActionResult> GuardarCompraCliente([FromBody] dtoArticuloCliente articuloCliente)
        {
            Response resp = await _articuloClienteServicios.GuardarArticuloClienteAsync(articuloCliente);
            return Ok(resp);
        }


        [HttpGet("{id:long}")]
        public async Task<IActionResult> ObtenerCompraCliente(long id)
        {
            Response resp = await _articuloClienteServicios.ObtenerCompraClienteAsync(id);
            return Ok(resp);
        }


        [HttpGet("lista")]
        public async Task<IActionResult> Lista(
            [FromQuery] int iTake = 5,
            [FromQuery] int iSkip = 0)
        {
            Response resp = await _articuloClienteServicios.ListaAsync(iTake, iSkip);
            return Ok(resp);
        }
    }
}
