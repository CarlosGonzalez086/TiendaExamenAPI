using Microsoft.AspNetCore.Mvc;
using TiendaExamenAPI.DbData.DtoModels.ArticuloCliente;
using TiendaExamenAPI.Services.ArticuloCliente;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TiendaExamenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraClienteController : ControllerBase
    {
        ArticuloClienteServicios articuloClienteServicios = new();
        [HttpPost]
        public async Task<IActionResult> guardarCompraCliente(dtoArticuloCliente articuloCliente)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await articuloClienteServicios.guardarArticuloCliente(articuloCliente);
            return Ok(resp);
        }
        [HttpGet]
        public async Task<IActionResult> obtenerCompraCliente(long id) 
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await articuloClienteServicios.obtenerCompraCliente(id);
            return Ok(resp);
        }
        [HttpGet("lista")]
        public async Task<IActionResult> lista(int iTake = 5, int iSkip = 0)
        {
            DbData.DtoModels.Response.Response resp = new();
            resp = await articuloClienteServicios.lista(iTake, iSkip);
            return Ok(resp);
        }
    }
}
