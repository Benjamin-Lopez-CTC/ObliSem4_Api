using Microsoft.EntityFrameworkCore;
using ObliSem4Api.Data;
using ObliSem4Api.DTO.Filters;
using ObliSem4Api.DTO.Tasks;
using ObliSem4Api.Interfaces;
using ObliSem4Api.Models;

namespace ObliSem4Api.Repository
{
    public class TasksRepository : ITasksRepository
    {
        private readonly AppDbContext _context;

        public TasksRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene todas las tareas del usuario actualmente en sesion, con filtros opcionales.
        public async Task<PagedResult<TaskReadDto>> GetFilteredAsync(int userId, TaskFilterDto filter)
        {
            IQueryable<Tasks> query = _context.Tasks
                .Where(t => t.UserId == userId)
                .AsNoTracking();

            //Validaciones

            if (filter.Page < 1)
            {
                filter.Page = 1;
            }

            if (filter.PageSize < 1)
            {
                filter.PageSize = 10;
            }

            if (filter.PageSize > 100)
            {
                filter.PageSize = 100;
            }

            if (filter.ProjectId.HasValue)
            {
                query = query.Where(e => e.ProjectId == filter.ProjectId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query = query.Where(e => e.Title.Contains(filter.Title));
            }

            if (!string.IsNullOrWhiteSpace(filter.Description))
            {
                query = query.Where(e => e.Description.Contains(filter.Description));
            }

            if (!string.IsNullOrWhiteSpace(filter.Priority))
            {
                query = query.Where(e => e.Priority == filter.Priority);
            }

            if (!string.IsNullOrWhiteSpace(filter.AccentColor))
            {
                query = query.Where(e => e.AccentColor == filter.AccentColor);
            }

            if (filter.DueDate.HasValue)
            {
                query = query.Where(e => e.DueDate.HasValue && e.DueDate.Value.Date == filter.DueDate.Value.Date);
            }

            if (filter.CompletedAt.HasValue)
            {
                query = query.Where(e => e.CompletedAt.HasValue && e.CompletedAt.Value.Date == filter.CompletedAt.Value.Date);
            }

            if (filter.CreatedAt.HasValue)
            {
                query = query.Where(e => e.CreatedAt.Date == filter.CreatedAt.Value.Date);
            }

            if (filter.IsDeleted)
            {
                query = query.IgnoreQueryFilters()
                    .Where(e => e.UserId == userId && e.IsDeleted == true);
            }

            if (filter.HasSubtasks)
            {
                query = query.Where(e => e.Subtasks!.Any());
            }

            if (filter.HasLocation)
            {
                query = query.Where(e => e.ZoneLatitude != null && e.ZoneLongitude != null);
            }

            if (filter.HasAttachment)
            {
                query = query.Where(e => e.AttachmentKey != null);
            }

            //Cantidad de registros, paginacion y query final

            int total = await query.CountAsync();

            query = query.OrderBy(e => e.Title).ThenBy(e => e.Id);

            int salto = (filter.Page - 1) * filter.PageSize;

            var tasks = await query
            .Skip(salto)
            .Take(filter.PageSize)
            .Select(e => new TaskReadDto
            {
                IdProject = e.ProjectId,
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
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        // Crea una nueva tarea para el usuario actualmente en sesion.
        public async Task CreateAsync(Tasks tarea)
        {
            await _context.Tasks.AddAsync(tarea);
            await _context.SaveChangesAsync();
        }
    }
}