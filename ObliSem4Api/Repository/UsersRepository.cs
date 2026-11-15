using Microsoft.EntityFrameworkCore;
using ObliSem4Api.Data;
using ObliSem4Api.Interfaces;
using ObliSem4Api.Models;

namespace ObliSem4Api.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _context;

        public UsersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Users>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
