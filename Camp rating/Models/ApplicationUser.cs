using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
namespace Camp_rating.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [PersonalData]
        public string FirstName { get; set; }

        [Required]
        [PersonalData]
        public string LastName { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

    }
}
