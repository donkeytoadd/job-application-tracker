namespace job_application_tracker.Tests.Controllers
{
    using job_application_tracker.Business.Creators.Interfaces;
    using job_application_tracker.Business.Getters.Interfaces;
    using job_application_tracker.Business.Updaters.Interfaces;
    using job_application_tracker.Controllers;
    using job_application_tracker.Data.Entities;
    using job_application_tracker.Data.Enums;
    using Microsoft.AspNetCore.Mvc;
    using Moq;

    public class JobApplicationControllerTests : TestBase<JobApplicationController>
    {
        [Fact]
        public void GetJobApplicationsByUserId_ReturnsOkWithApplicationList()
        {
            // Arrange
            var userId = 1;
            var expectedApplications = new List<JobApplication>
            {
                new()
                {
                    Id = 1, 
                    UserId = userId, 
                    Company = "Google", 
                    Role = "Senior Engineer"
                },
                new()
                {
                    Id = 2, 
                    UserId = userId, 
                    Company = "Microsoft", 
                    Role = "Software Engineer II"
                }
            };

            this.automocker.GetMock<IApplicationGetter>()
                .Setup(x => x.GetApplicationsByUserId(userId))
                .Returns(expectedApplications);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.GetJobApplicationsByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var applications = Assert.IsType<List<JobApplication>>(okResult.Value);
            Assert.Equal(2, applications.Count);
        }

        [Fact]
        public void GetJobApplicationsByUserId_ReturnsEmptyListWhenUserHasNoApplications()
        {
            // Arrange
            var userId = 99;

            this.automocker.GetMock<IApplicationGetter>()
                .Setup(x => x.GetApplicationsByUserId(userId))
                .Returns(new List<JobApplication>());

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.GetJobApplicationsByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var applications = Assert.IsType<List<JobApplication>>(okResult.Value);
            Assert.Empty(applications);
        }

        [Fact]
        public void CreateJobApplication_ReturnsOkWithCreatedApplication()
        {
            // Arrange
            var request = new CreateApplication
            {
                UserId = 1,
                Company = "Stripe",
                Role = "Backend Engineer",
                JobUrl = "https://stripe.com/jobs/1",
                Notes = "Payments infra team"
            };

            var expectedApplication = new JobApplication
            {
                Id = 1,
                UserId = request.UserId,
                Company = request.Company,
                Role = request.Role
            };

            this.automocker.GetMock<IApplicationCreator>()
                .Setup(x => x.CreateJobApplication(request))
                .Returns(expectedApplication);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.CreateJobApplication(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var application = Assert.IsType<JobApplication>(okResult.Value);
            Assert.Equal(1, application.Id);
            Assert.Equal("Stripe", application.Company);
        }

        [Fact]
        public void UpdateApplicationStatus_ReturnsOkWithUpdatedApplication()
        {
            // Arrange
            var applicationId = 1;
            var request = new UpdateApplicationStatus { NewStatus = ApplicationStatus.Interview };

            var expectedApplication = new JobApplication
            {
                Id = applicationId,
                UserId = 1,
                Company = "Google",
                Role = "Senior Engineer",
                Status = ApplicationStatus.Interview
            };

            this.automocker.GetMock<IApplicationUpdater>()
                .Setup(x => x.UpdateApplicationStatus(applicationId, request.NewStatus))
                .Returns(expectedApplication);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.UpdateApplicationStatus(applicationId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var application = Assert.IsType<JobApplication>(okResult.Value);
            Assert.Equal(ApplicationStatus.Interview, application.Status);
        }

        [Fact]
        public void UpdateApplicationStatus_ReturnsNotFoundWhenApplicationDoesNotExist()
        {
            // Arrange
            var applicationId = 99;
            var request = new UpdateApplicationStatus { NewStatus = ApplicationStatus.Screening };

            this.automocker.GetMock<IApplicationUpdater>()
                .Setup(x => x.UpdateApplicationStatus(applicationId, request.NewStatus))
                .Returns((JobApplication?)null);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.UpdateApplicationStatus(applicationId, request);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}