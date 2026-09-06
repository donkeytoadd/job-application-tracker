namespace job_application_tracker.Data.Queries
{
    using Entities;
    using Enums;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class UpdateApplicationStatusQuery : IUpdateApplicationStatusQuery
    {
        private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

        public UpdateApplicationStatusQuery(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public JobApplication? Execute(int applicationId, ApplicationStatus newStatus)
        {
            using var context = this.contextFactory.CreateDbContext();

            var application = context.JobApplication
                .Include(a => a.StatusHistory)
                .FirstOrDefault(a => a.Id == applicationId);

            if (application == null)
                return null;

            application.Status = newStatus;
            application.StatusHistory.Add(new ApplicationStatusHistory { Status = newStatus });

            context.SaveChanges();

            return application;
        }
    }
}