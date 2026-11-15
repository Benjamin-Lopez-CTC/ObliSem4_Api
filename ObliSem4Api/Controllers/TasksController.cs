using Microsoft.AspNetCore.Mvc;
using ObliSem4Api.DTO.Tasks;
using ObliSem4Api.Interfaces;

namespace ObliSem4Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _service;
        public TasksController(ITasksService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskReadDto>>> GetAll()
        {
            try
            {
                var tareas = await _service.GetAll();
                return Ok(tareas);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
