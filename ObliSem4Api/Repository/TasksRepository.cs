using Microsoft.EntityFrameworkCore;
using ObliSem4Api.Data;
using ObliSem4Api.DTO.Filters;
using ObliSem4Api.DTO.Tasks;
using ObliSem4Api.Interfaces;
using ObliSem4Api.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ObliSem4Api.Repository
{
    public class TasksRepository : ITasksRepository
    {
        private readonly AppDbContext _context;

        public TasksRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TaskReadDto>> GetAll(
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
            )
        {
            IQueryable<Tasks> query = _context.Tasks.AsNoTracking();

            //Validaciones

            if(page < 1)
            {
                page = 1;
            }
                
            if (pageSize < 1) 
            { 
                pageSize = 10; 
            }
                
            if(pageSize > 100) 
            {
                pageSize = 100;
            }
                
            if (!string.IsNullOrWhiteSpace(Title))
            {
                query = query.Where(e => e.Title.Contains(Title));
            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                query = query.Where(e => e.Description.Contains(Description));
            }

            if (!string.IsNullOrWhiteSpace(Priority))
            {
                query = query.Where(e => e.Priority.Contains(Priority));
            }

            if (!string.IsNullOrWhiteSpace(AccentColor))
            {
                query = query.Where(e => e.AccentColor == AccentColor);
            }

            if (DueDate.HasValue)
            {
                query = query.Where(e => e.DueDate.HasValue && e.DueDate.Value.Date == DueDate.Value.Date);
            }

            if (CompletedAt.HasValue)
            {
                query = query.Where(e => e.CompletedAt.HasValue && e.CompletedAt.Value.Date == CompletedAt.Value.Date);
            }

            if (CreatedAt.HasValue)
            {
                query = query.Where(e => e.CreatedAt.Date == CreatedAt.Value.Date);
            }

            if (IsDeleted.HasValue)
            {
                query = query.Where(e => e.IsDeleted == IsDeleted.Value);
            }

            if (HasLocation.HasValue)
            {
                query = query.Where(e => HasLocation.Value ? e.ZoneLatitude != null && e.ZoneLongitude != null : e.ZoneLatitude == null || e.ZoneLongitude == null);
            }

            if (HasAttachment.HasValue)
            {
                query = query.Where(e => HasAttachment.Value ? e.AttachmentKey != null : e.AttachmentKey == null);
            }

            //Cantidad de registros, paginacion y query final

            int total = await query.CountAsync();

            query = query.OrderBy(e => e.Title).ThenBy(e => e.Id);

            int salto = (page - 1) * pageSize;

            var tasks = await query
            .Skip(salto)
            .Take(pageSize)
            .Select(e => new TaskReadDto
            {
                Title = e.Title,
                Description = e.Description,
                Priority = e.Priority,
                DueDate = e.DueDate,
                CompletedAt = e.CompletedAt,
                IsDeleted = e.IsDeleted,
                DueEmailSent = e.DueEmailSent,
                AccentColor = e.AccentColor,
                CreatedAt = e.CreatedAt,

                // Ubicacion (opcional)
                ZoneLatitude = e.ZoneLatitude,
                ZoneLongitude = e.ZoneLongitude,
                ZoneRadiusMeters = e.ZoneRadiusMeters,
                ZoneAddress = e.ZoneAddress,

                // Archivo (opcional)
                AttachmentKey = e.AttachmentKey,
                AttachmentName = e.AttachmentName,
                AttachmentType = e.AttachmentType,
                AttachmentUrl = e.AttachmentUrl
            })       
            .ToListAsync();

            return new PagedResult<TaskReadDto>
            {
                Items = tasks,
                Total = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}