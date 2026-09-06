namespace job_application_tracker.Data
{
    using Entities;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<JobApplication> JobApplication { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<ApplicationStatusHistory> ApplicationStatusHistory { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
                entity.Property(u => u.PasswordHash).IsRequired();

                entity.HasMany(u => u.Applications)
                    .WithOne()
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<JobApplication>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Company).IsRequired().HasMaxLength(256);
                entity.Property(a => a.Role).IsRequired().HasMaxLength(256);
                entity.Property(a => a.JobUrl).HasMaxLength(2048);
                entity.Property(a => a.Status).HasConversion<int>();

                entity.HasMany(a => a.StatusHistory)
                    .WithOne(h => h.Application)
                    .HasForeignKey(h => h.ApplicationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApplicationStatusHistory>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Status).HasConversion<int>();
            });
        }
    }
}