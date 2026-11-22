using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObliSem4Api.DTO.Filters;
using ObliSem4Api.Interfaces;
using ObliSem4Api.DTO.Projects;

namespace ObliSem4Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _service;
        public ProjectsController(IProjectsService service)
        {
            _service = service;
        }

        // Obtiene el id del usuario actualmente en sesion
        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Token invalido");
            }
            return userId;
        }

        // Get de todos los proyectos, solo para admin
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<ActionResult<List<ProjectReadDTO>>> GetAll()
        {
            try
            {
                var proyectos = await _service.GetAllAsync();
                return Ok(proyectos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/projects
        [HttpGet]
        public async Task<ActionResult<List<ProjectReadDTO>>> GetFiltered([FromQuery] ProjectFilterDto filter)
        {
            try
            {
                var userId = GetCurrentUserId();
                var projects = await _service.GetFilteredAsync(userId, filter);
                return Ok(projects);
            } catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            } catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            } catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/projects/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProjectReadDTO>> GetById(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var proyecto = await _service.GetByIdAsync(id, userId);
                return Ok(proyecto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/projects
        [HttpPost]
        public async Task<ActionResult<ProjectReadDTO>> Create([FromBody] ProjectCreateDTO dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var creado = await _service.CreateAsync(userId, dto);
                return StatusCode(201, creado);
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

        // PATCH: api/project/{id}/color?color=[valor] (el valor se puede omitir, queda null)
        [HttpPatch("{id:int}/color")]
        public async Task<IActionResult> UpdateColor(int id, [FromQuery] string? color)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _service.UpdateColorAsync(id, userId, color);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PATCH: api/projects/{id}/status
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] ProjectStatusDTO dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _service.UpdateStatusAsync(id, userId, dto.Status);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/projects/{id} (soft delete => va a la papelera)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _service.SoftDeleteAsync(id, userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PATCH: api/projects/{id}/restore (restaurar desde la papelera)
        [HttpPatch("{id:int}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _service.RestoreAsync(id, userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/projects/{id}/hard (borrado de la base de datos, solo admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}/hard")]
        public async Task<IActionResult> HardDelete(int id)
        {
            try
            {
                await _service.HardDeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
