using Microsoft.AspNetCore.Mvc;
using TiendaExamenAPI.DbData.DtoModels.articulos;
using TiendaExamenAPI.Services.Articulos;

namespace TiendaExamenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {

        ArticulosServicios articulos = new();

        [HttpPost]
        public async Task<IActionResult> guardarArticulo(dtoArticulos data)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await articulos.guardarArticulo(data);
            return Ok(resp);
        }

        [HttpPut]
        public async Task<IActionResult> actualizarArticulo(dtoArticulos data)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await articulos.actualizarArticulo(data);
            return Ok(resp);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> obtenerArticulo(long id)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await articulos.obtenerArticulo(id);
            return Ok(resp);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarArticulo(long id)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await articulos.EliminarArticulo(id);
            return Ok(resp);
        }

        [HttpGet("lista")]
        public async Task<IActionResult> lista(int iTake = 5, int iSkip = 0)
        {
            DbData.DtoModels.Response.Response resp = new();
             resp = await articulos.lista(iTake, iSkip);
            return Ok(resp);
        }
    }
}
