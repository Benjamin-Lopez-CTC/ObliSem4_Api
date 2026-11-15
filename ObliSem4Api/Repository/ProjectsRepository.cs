using Microsoft.EntityFrameworkCore;
using ObliSem4Api.Data;
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

        public async Task<List<Projects>> GetAll()
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .ToListAsync();
        }
    }
}
