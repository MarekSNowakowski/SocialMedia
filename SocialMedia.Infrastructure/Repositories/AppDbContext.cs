using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Domain;

namespace SocialMedia.Infrastructure.Repositories
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<UserData> UserData { get; set; }
        public DbSet<Votes> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne(b => b.Votes)
                .WithOne(i => i.Post)
                .HasForeignKey<Votes>(b => b.PostId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
