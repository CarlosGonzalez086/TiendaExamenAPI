using Microsoft.AspNetCore.Mvc;
using TiendaExamenAPI.DbData.DtoModels.articulos;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.Modelos;
using TiendaExamenAPI.Services.Articulos;

namespace TiendaExamenAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticulosController : ControllerBase
    {
        private readonly ArticulosServicios _articulosServicios;

        public ArticulosController(ArticulosServicios articulosServicios)
        {
            _articulosServicios = articulosServicios;
        }

        [HttpPost]
        public async Task<IActionResult> GuardarArticulo([FromBody] dtoArticulos data)
        {
            Response resp = await _articulosServicios.GuardarArticuloAsync(data);
            return Ok(resp);
        }


        [HttpPut]
        public async Task<IActionResult> ActualizarArticulo([FromBody] dtoArticulos data)
        {
            Response resp = await _articulosServicios.ActualizarArticuloAsync(data);
            return Ok(resp);
        }


        [HttpGet("{id:long}")]
        public async Task<IActionResult> ObtenerArticulo(long id)
        {
            Response resp = await _articulosServicios.ObtenerArticuloAsync(id);
            return Ok(resp);
        }


        [HttpDelete("{id:long}")]
        public async Task<IActionResult> EliminarArticulo(long id)
        {
            Response resp = await _articulosServicios.EliminarArticuloAsync(id);
            return Ok(resp);
        }
        [HttpGet("all")]
        public async Task<IActionResult> ObtenerArticulos()
        {
            Response resp = await _articulosServicios.ObtenerArticulosAsync();
            return Ok(resp);
        }

        [HttpGet("lista")]
        public async Task<IActionResult> Lista(
            [FromQuery] int iTake = 5,
            [FromQuery] int iSkip = 0)
        {
            Response resp = await _articulosServicios.ListaAsync(iTake, iSkip);
            return Ok(resp);
        }
    }
}
