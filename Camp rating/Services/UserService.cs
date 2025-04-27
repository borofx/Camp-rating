using Camp_rating.Data;
using Camp_rating.Models;
using Microsoft.EntityFrameworkCore;
using Camp_rating.Services;

namespace Camp_rating.Services
{
        public class UserService : IUserService
        {
            private readonly ApplicationDbContext _context;

            public UserService(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
            {
                return await _context.Users.ToListAsync();
            }

            public async Task<ApplicationUser> GetUserByIdAsync(string id)
            {
                return await _context.Users.FindAsync(id);
            }

            public async Task UpdateUserAsync(ApplicationUser user)
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            public async Task DeleteUserAsync(string id)
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
        }
}
