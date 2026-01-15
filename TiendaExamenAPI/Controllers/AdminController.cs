using Microsoft.AspNetCore.Mvc;
using TiendaExamenAPI.DbData.DtoModels;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.Services;

namespace TiendaExamenAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _service;

        public AdminController(AdminService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<Response> ObtenerTodos()
        {
            var response = _service.ObtenerTodos();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<Response> ObtenerPorId(long id)
        {
            var response = _service.ObtenerAdminPorId(id);
            return StatusCode(int.Parse(response.codigo), response);
        }

        [HttpPost]
        public ActionResult<Response> Crear([FromBody] AdminCreateDto dto)
        {
            var response = _service.CrearAdmin(dto);
            return StatusCode(int.Parse(response.codigo), response);
        }

        [HttpPut("{id}")]
        public ActionResult<Response> Actualizar(long id, [FromBody] AdminUpdateDto dto)
        {
            var response = _service.ActualizarAdmin(id, dto);
            return StatusCode(int.Parse(response.codigo), response);
        }

        [HttpDelete("{id}")]
        public ActionResult<Response> Eliminar(long id)
        {
            var response = _service.EliminarAdmin(id);
            return StatusCode(int.Parse(response.codigo), response);
        }
    }
}
