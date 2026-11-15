using Microsoft.AspNetCore.Mvc;
using ObliSem4Api.Interfaces;
using ObliSem4Api.DTO.Projects;

namespace ObliSem4Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _service;
        public ProjectsController(IProjectsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectReadDTO>>> GetAll()
        {
            try
            {
                var proyectos = await _service.GetAll();
                return Ok(proyectos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
