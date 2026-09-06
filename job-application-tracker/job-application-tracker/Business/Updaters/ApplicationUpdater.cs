namespace job_application_tracker.Business.Updaters
{
    using Data.Entities;
    using Data.Enums;
    using Data.Queries.Interfaces;
    using Interfaces;

    public class ApplicationUpdater : IApplicationUpdater
    {
        private readonly IUpdateApplicationStatusQuery updateApplicationStatusQuery;

        public ApplicationUpdater(IUpdateApplicationStatusQuery updateApplicationStatusQuery)
        {
            this.updateApplicationStatusQuery = updateApplicationStatusQuery;
        }

        public JobApplication? UpdateApplicationStatus(int applicationId, ApplicationStatus newStatus)
        {
            return this.updateApplicationStatusQuery.Execute(applicationId, newStatus);
        }
    }
}