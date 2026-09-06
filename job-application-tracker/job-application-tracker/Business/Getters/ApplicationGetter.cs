namespace job_application_tracker.Business.Getters
{
    using Data.Entities;
    using Data.Queries.Interfaces;
    using Interfaces;

    public class ApplicationGetter : IApplicationGetter
    {
        private readonly IGetApplicationsByUserIdQuery getApplicationsByUserIdQuery;

        public ApplicationGetter(IGetApplicationsByUserIdQuery getApplicationsByUserIdQuery)
        {
            this.getApplicationsByUserIdQuery = getApplicationsByUserIdQuery;
        }

        public List<JobApplication> GetApplicationsByUserId(int userId)
        {
            var applicationList = this.getApplicationsByUserIdQuery.Execute(userId);
            return applicationList.Count > 0 ? applicationList : new List<JobApplication>();
        }
    }
}