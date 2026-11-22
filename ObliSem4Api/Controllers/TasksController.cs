using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObliSem4Api.DTO.Filters;
using ObliSem4Api.DTO.Tasks;
using ObliSem4Api.Interfaces;

namespace ObliSem4Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _service;
        public TasksController(ITasksService service)
        {
            _service = service;
        }

        // Obtiene el id del usuario actualmente en sesion
        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(idClaim, out var userId))
            {
                throw new UnauthorizedAccessException(
                    "Token invalido");
            }

            return userId;
        }

        // GET: api/tasks
        // Obtiene todas las tareas del usuario actualmente en sesion, con filtros opcionales.
        [HttpGet]
        public async Task<ActionResult<PagedResult<TaskReadDto>>> GetAll([FromQuery] TaskFilterDto filter)
        {
            try
            {
                var userId = GetCurrentUserId();
                var tareas = await _service.GetFilteredAsync(userId, filter);
                return Ok(tareas);
            } 
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            } 
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            } 
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/tasks
        // Crea una nueva tarea para el usuario actualmente en sesion.
        [HttpPost]
        public async Task<ActionResult<TaskReadDto>> Create([FromBody] TaskCreateDto taskCreateDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var createdTask = await _service.CreateAsync(userId, taskCreateDto);
                return Ok(createdTask);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}