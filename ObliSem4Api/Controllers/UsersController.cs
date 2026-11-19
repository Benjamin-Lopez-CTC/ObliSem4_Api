using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

        // Solo los administradores pueden obtener la lista de todos los usuarios registrados
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<UserReadDto>>> GetAll()
        {
            try
            {
                var users = await _service.GetAllAsync();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // Metodo de registro de usuario
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] UserRegisterDto dto)
        {
            try
            {
                var res = await _service.RegisterAsync(dto);
                return StatusCode(201, res);
            } catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            } catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            } catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // Metodo de login de usuario
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var res = await _service.LoginAsync(dto);
                return Ok(res);
            } catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            } catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            } 
        }

        // Devuelve el usuario actualmente en sesion
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserReadDto>> GetMe()
        {
            try
            {
                var userId = GetCurrentUserId();
                var user = await _service.GetByIdAsync(userId);
                return Ok(user);
            } catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            } catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            } catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // Actualiza el tema del usuario en sesion
        [Authorize]
        [HttpPatch("theme")]
        public async Task<IActionResult> UpdateTheme([FromBody] UserThemeDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _service.UpdateThemeAsync(userId, dto);
                return NoContent();
            } catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            } catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            } catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            } catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
