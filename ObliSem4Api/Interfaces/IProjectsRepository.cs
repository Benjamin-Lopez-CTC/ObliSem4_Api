using ObliSem4Api.Models;
using ObliSem4Api.DTO.Projects;

namespace ObliSem4Api.Interfaces
{
    public interface IProjectsRepository
    {
        Task<List<Projects>> GetAll();
    }
}
