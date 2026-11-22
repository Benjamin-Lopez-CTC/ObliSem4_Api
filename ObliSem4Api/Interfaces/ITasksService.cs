using ObliSem4Api.DTO.Filters;
using ObliSem4Api.DTO.Tasks;

namespace ObliSem4Api.Interfaces
{
    public interface ITasksService
    {
        //Get all con filtros opcionales
        Task<PagedResult<TaskReadDto>> GetAll(
        string? Title,
        string? Description,
        string? Priority,
        DateTime? DueDate,
        DateTime? CompletedAt,
        bool? IsDeleted,
        string? AccentColor,
        DateTime? CreatedAt,
        bool? HasLocation,
        bool? HasAttachment,
        int page = 1,
        int pageSize = 10
        );
    }
}
