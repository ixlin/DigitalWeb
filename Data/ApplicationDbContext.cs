using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DigitalSolutionsWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace DigitalSolutionsWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置ContactMessage实体
            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SubmittedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
