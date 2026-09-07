namespace job_application_tracker.Controllers
{
    using Business.Creators.Interfaces;
    using Business.Getters.Interfaces;
    using Business.Updaters.Interfaces;
    using Data.Entities;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class JobApplicationController : ControllerBase
    {
        private readonly ILogger<JobApplicationController> logger;
        private readonly IApplicationGetter applicationGetter;
        private readonly IApplicationCreator applicationCreator;
        private readonly IApplicationUpdater applicationUpdater;

        public JobApplicationController(
            ILogger<JobApplicationController> logger,
            IApplicationGetter applicationGetter,
            IApplicationCreator applicationCreator,
            IApplicationUpdater applicationUpdater)
        {
            this.logger = logger;
            this.applicationGetter = applicationGetter;
            this.applicationCreator = applicationCreator;
            this.applicationUpdater = applicationUpdater;
        }

        [HttpGet("user/{userId}")]
        public ActionResult<List<JobApplication>> GetJobApplicationsByUserId(int userId)
        {
            logger.LogInformation("Getting all applications for UserId {UserId}", userId);
            var applicationList = this.applicationGetter.GetApplicationsByUserId(userId);
            return Ok(applicationList);
        }

        [HttpGet("{id}")]
        public ActionResult<JobApplication> GetJobApplicationById(int id)
        {
            logger.LogInformation("Getting JobApplication with Id {Id}", id);
            var application = this.applicationGetter.GetApplicationById(id);

            if (application == null)
                return NotFound();

            return Ok(application);
        }

        [HttpPost]
        public ActionResult<JobApplication> CreateJobApplication(CreateApplication createApplicationRequest)
        {
            logger.LogInformation("Creating new JobApplication for UserId {UserId}", createApplicationRequest.UserId);
            var created = this.applicationCreator.CreateJobApplication(createApplicationRequest);
            return Ok(created);
        }

        [HttpPatch("{id}/status")]
        public ActionResult<JobApplication> UpdateApplicationStatus(int id, UpdateApplicationStatus request)
        {
            logger.LogInformation("Updating status for ApplicationId {Id} to {Status}", id, request.NewStatus);
            var updated = this.applicationUpdater.UpdateApplicationStatus(id, request.NewStatus);

            if (updated == null)
                return NotFound();

            return Ok(updated);
        }
    }
}