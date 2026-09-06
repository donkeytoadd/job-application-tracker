namespace job_application_tracker.Data.Queries
{
    using System.Net.Mime;
    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class GetApplicationsByUserIdQuery : IGetApplicationsByUserIdQuery
    {
        private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

        public GetApplicationsByUserIdQuery(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public List<JobApplication> Execute(int userId)
        {
            using (var context = this.contextFactory.CreateDbContext())
            {
                return context.JobApplication
                    .Where(x => x.UserId == userId)
                    .Include(x => x.StatusHistory)
                    .OrderByDescending(x => x.Id)
                    .ToList();
            }
        }
    }
}