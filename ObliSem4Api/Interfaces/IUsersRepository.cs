using ObliSem4Api.Models;

namespace ObliSem4Api.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<Users>> GetAllAsync();
        Task<Users?> GetByEmailAsync(string email);
        Task<Users?> GetByIdAsync(int id);
        Task<bool> UpdateThemeAsync(int id, string? theme);
        Task AddAsync(Users user);
    }
}
