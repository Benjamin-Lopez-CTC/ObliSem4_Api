using ObliSem4Api.DTO.Users;

namespace ObliSem4Api.Interfaces
{
    public interface IUsersService
    {
        Task<List<UserReadDto>> GetAll();
    }
}
