using Microsoft.EntityFrameworkCore;
using ObliSem4Api.Data;
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

        public async Task<List<Tasks>> GetAll()
        {
            return await _context.Tasks.ToListAsync();
        }
    }
}
