using Camp_rating.Models;
using Camp_rating.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camp_rating.Services
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task UpdateUserAsync(ApplicationUser user);
        Task DeleteUserAsync(string id);
    }
}
