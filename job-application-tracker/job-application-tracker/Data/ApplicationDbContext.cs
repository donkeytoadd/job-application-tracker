namespace job_application_tracker.Data
{
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        //DB tables go here

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}