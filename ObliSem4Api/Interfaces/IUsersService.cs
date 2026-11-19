using ObliSem4Api.DTO.Users;

namespace ObliSem4Api.Interfaces
{
    public interface IUsersService
    {
        Task<List<UserReadDto>> GetAllAsync();
        Task<AuthResponseDto> RegisterAsync(UserRegisterDto dto);
        Task<AuthResponseDto> LoginAsync(UserLoginDto dto);
        Task<UserReadDto?> GetByIdAsync(int id);
        Task UpdateThemeAsync(int id, UserThemeDto dto);
    }
}
