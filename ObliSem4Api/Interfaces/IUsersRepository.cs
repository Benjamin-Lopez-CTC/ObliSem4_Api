using ObliSem4Api.Models;
using ObliSem4Api.DTO.Users;

namespace ObliSem4Api.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<Users>> GetAll();
    }
}
