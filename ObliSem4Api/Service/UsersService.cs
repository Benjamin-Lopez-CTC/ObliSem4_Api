using ObliSem4Api.DTO.Tasks;
using ObliSem4Api.DTO.Users;
using ObliSem4Api.Interfaces;
using ObliSem4Api.Models;

namespace ObliSem4Api.Service
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;

        public UsersService(IUsersRepository repository)
        {
            _repository = repository;
        }

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

        public async Task<List<UserReadDto>> GetAll()
        {
            var users = await _repository.GetAll();

            return users.Select(ToReadDto).ToList();
        }
    }
}

