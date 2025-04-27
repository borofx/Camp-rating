using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Sockets;
using System.Security.Cryptography.Xml;
using Camp_rating.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace Camp_rating.Data
{
    public class ApplicationDbContext :  IdentityDbContext<ApplicationUser>
    {
        public DbSet<Campsite> Campsites { get; set; }
        public DbSet<Review> Reviews { get; set; }//svurzvashta tablica (izpolzvam latinica, zashtoto nqma kirilizaciq)

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }        


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Review>()
                .HasOne(o => o.Campsite)
                .WithMany()
            .HasForeignKey("CampsiteId");

            builder.Entity<Review>()
                .HasOne(o => o.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey("UserId");
        }
    }
}
