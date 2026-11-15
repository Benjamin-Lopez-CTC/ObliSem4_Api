using Microsoft.AspNetCore.Mvc;
using ObliSem4Api.DTO.Users;
using ObliSem4Api.Interfaces;

namespace ObliSem4Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _service;
        public UsersController(IUsersService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserReadDto>>> GetAll()
        {
            try
            {
                var users = await _service.GetAll();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
