using CAT3B.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CAT3B.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Post>()
                .Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<Post>()
                .Property(p => p.ImageUrl)
                .IsRequired();

            builder.Entity<Post>()
                .Property(p => p.PublishedAt)
                .IsRequired();
        }
    }
}