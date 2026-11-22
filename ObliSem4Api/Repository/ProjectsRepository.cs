using Microsoft.EntityFrameworkCore;
using ObliSem4Api.Data;
using ObliSem4Api.DTO.Filters;
using ObliSem4Api.Interfaces;
using ObliSem4Api.Models;

namespace ObliSem4Api.Repository
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly AppDbContext _context;

        public ProjectsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Projects>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .ToListAsync();
        }

        public async Task<List<Projects>> GetFilteredAsync(int userId, ProjectFilterDto filter)
        {
            IQueryable<Projects> query = _context.Projects
                .Where(p => p.UserId == userId)
                .Include(p => p.Tasks)
                .AsNoTracking();

            // Validaciones
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(p => p.Name.Contains(filter.Name));
            }

            if (!string.IsNullOrWhiteSpace(filter.Description))
            {
                query = query.Where(p => p.Description.Contains(filter.Description));
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(p => p.Status == filter.Status);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(p => p.StartDate >= filter.StartDate);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(p => p.EndDate <= filter.EndDate);
            }

            if (!string.IsNullOrWhiteSpace(filter.AccentColor))
            {
                query = query.Where(p => p.AccentColor == filter.AccentColor);
            }

            if (filter.IsDeleted)
            {
                query = query.IgnoreQueryFilters()
                    .Where(p => p.UserId == userId && p.IsDeleted == true);
            }

            if (filter.CreatedAt.HasValue)
            {
                query = query.Where(p => p.CreatedAt.Date <= filter.CreatedAt.Value.Date);
            }

            // Switch para tipo de ordenado
            query = (filter.SortBy?.ToLower(), filter.SortDesc) switch
            {
                ("name", true) => query.OrderByDescending(p => p.Name),
                ("name", false) => query.OrderBy(p => p.Name),
                ("status", true) => query.OrderByDescending(p => p.Status),
                ("status", false) => query.OrderBy(p => p.Status),
                ("startdate", true) => query.OrderByDescending(p => p.StartDate),
                ("startdate", false) => query.OrderBy(p => p.StartDate),
                ("enddate", true) => query.OrderByDescending(p => p.EndDate),
                ("enddate", false) => query.OrderBy(p => p.EndDate),
                ("createdat", true) => query.OrderByDescending(p => p.CreatedAt),
                ("createdat", false) => query.OrderBy(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            return await query.ToListAsync();

        }

        public async Task<Projects?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> UpdateColorAsync(int id, int userId, string? color)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Id == id);

            if (project == null) return false;

            project.AccentColor = color;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task CreateAsync(Projects project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateStatusAsync(int id, int userId, string status)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (project == null) return false;
            if (project.Status.ToLower() == status.ToLower()) return true;

            project.Status = status;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id, int userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (project == null) return false;

            project.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestoreAsync(int id, int userId)
        {
            var project = await _context.Projects.IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => (p.Id == id && p.UserId == userId) && p.IsDeleted == true);

            if (project == null) return false;

            project.IsDeleted = false;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> HardDeleteAsync(int id)
        {
            var project = await _context.Projects
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
