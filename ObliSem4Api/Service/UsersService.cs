using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ObliSem4Api.DTO.Users;
using ObliSem4Api.Interfaces;
using ObliSem4Api.Models;

namespace ObliSem4Api.Service
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;
        private readonly IPasswordHasher<Users> _hasher;
        private readonly IConfiguration _config;

        public UsersService(IUsersRepository repository, IPasswordHasher<Users> hasher, IConfiguration config)
        {
            _repository = repository;
            _hasher = hasher;
            _config = config;
        }

        // Convierte el objeto usuario al dto de lectura para omitir informacion innecesaria
        private static UserReadDto ToReadDto(Users user)
        {
            return new UserReadDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Theme = user.Theme,
                CreatedAt = user.CreatedAt
            };
        }

        // Obtiene todos los usuarios de la base de datos
        public async Task<List<UserReadDto>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();

            return users.Select(ToReadDto).ToList();
        }

        // Encargado de validar y registrar el usuario
        public async Task<AuthResponseDto> RegisterAsync(UserRegisterDto dto)
        {
            var emailNormalizado = dto.Email.Trim().ToLowerInvariant();
            if(await _repository.GetByEmailAsync(emailNormalizado) != null)
            {
                throw new InvalidOperationException(
                    "El usuario ya existe"
                );
            }

            var user = new Users
            {
                Email = emailNormalizado,
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            await _repository.AddAsync(user);

            var (token, expiresAt) = GenerarToken(user);

            return new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = ToReadDto(user)
            };
        }

        // Encargado de validar e ingresar al usuario al sitio
        public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
        {
            var emailNormalizado = dto.Email.Trim().ToLowerInvariant();
            
            var user = await _repository.GetByEmailAsync(emailNormalizado);
            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Credenciales invalidas");
            }

            var resultado = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (resultado == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException(
                    "Credenciales invalidas");
            }

            var (token, expiresAt) = GenerarToken(user);

            return new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = ToReadDto(user)
            };
        }

        // Obtiene un usuario segun el Id
        public async Task<UserReadDto?> GetByIdAsync(int id)
        {
            ValidarId(id);

            var user = await _repository.GetByIdAsync(id);
            if(user == null)
            {
                throw new KeyNotFoundException(
                    $"No se encontro el usuario con el ID {id}."
                );
            }

            return ToReadDto(user);
        }

        // Valida que el tema haya sido cambiado exitosamente y que el usuario que cambio el tema exista
        public async Task UpdateThemeAsync(int id, UserThemeDto dto)
        {
            ValidarId(id);

            var actualizado = await _repository.UpdateThemeAsync(id, dto.Theme);
            if(!actualizado)
            {
                throw new KeyNotFoundException(
                    $"No se encontro el usuario con el ID {id}."
                );
            }
        }

        // Validacion basica de id reutilizable
        public static void ValidarId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "El ID del usuario es obligatorio"
                );
            }
        }

        private (string Token, DateTime ExpiresAt) GenerarToken(Users user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddDays(90);

            var jwt = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (token, expiresAt);
        }
    }
}

