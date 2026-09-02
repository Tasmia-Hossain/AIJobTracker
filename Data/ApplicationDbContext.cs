using AIJobTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AIJobTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobStatusHistory> JobStatusHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<JobStatusHistory>()
                .HasOne(h => h.Job)
                .WithMany(j => j.StatusHistory)
                .HasForeignKey(h => h.JobId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}