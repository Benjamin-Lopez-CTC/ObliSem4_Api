using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using ObliSem4Api.DTO.Filters;
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
        public async Task<ActionResult<PagedResult<TaskReadDto>>> GetAll(
               [FromQuery] string? Title = null,
               [FromQuery] string? Description = null,
               [FromQuery] string? Priority = null,
               [FromQuery] DateTime? DueDate = null,
               [FromQuery] DateTime? CompletedAt = null,
               [FromQuery] bool? IsDeleted = null,
               [FromQuery] string? AccentColor = null,
               [FromQuery] DateTime? CreatedAt = null,
               [FromQuery] bool? HasLocation = null,
               [FromQuery] bool? HasAttachment = null,
               [FromQuery] int page = 1,
               [FromQuery] int pageSize = 10
            )
        {
            try
            {
                var tareas = await _service.GetAll(
                    Title,
                    Description,
                    Priority,
                    DueDate,
                    CompletedAt,
                    IsDeleted,
                    AccentColor,
                    CreatedAt,
                    HasLocation,
                    HasAttachment,
                    page,
                    pageSize
                );

                return Ok(tareas);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}