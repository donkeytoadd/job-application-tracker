namespace job_application_tracker.Tests.Business.Creators
{
    using job_application_tracker.Business.Creators;
    using job_application_tracker.Data.Entities;
    using job_application_tracker.Data.Enums;
    using job_application_tracker.Data.Queries.Interfaces;
    using Moq;

    public class ApplicationCreatorTests : TestBase<ApplicationCreator>
    {
        [Fact]
        public void CreateJobApplication_MapsRequestFieldsCorrectly()
        {
            // Arrange
            var request = new CreateApplication
            {
                UserId = 1,
                Company = "Google",
                Role = "Senior Engineer",
                JobUrl = "https://careers.google.com/jobs/1",
                Notes = "Referral from John"
            };

            JobApplication? capturedApplication = null;

            this.automocker.GetMock<ICreateApplicationQuery>()
                .Setup(x => x.Execute(It.IsAny<JobApplication>()))
                .Callback<JobApplication>(a => capturedApplication = a)
                .Returns<JobApplication>(a => a);

            var sut = this.CreateTestSubject();

            // Act
            sut.CreateJobApplication(request);

            // Assert
            Assert.NotNull(capturedApplication);
            Assert.Equal(request.UserId, capturedApplication.UserId);
            Assert.Equal(request.Company, capturedApplication.Company);
            Assert.Equal(request.Role, capturedApplication.Role);
            Assert.Equal(request.JobUrl, capturedApplication.JobUrl);
            Assert.Equal(request.Notes, capturedApplication.Notes);
        }

        [Fact]
        public void CreateJobApplication_SeedsInitialAppliedStatusHistory()
        {
            // Arrange
            var request = new CreateApplication
            {
                UserId = 1, 
                Company = "Google", 
                Role = "Senior Engineer"
            };

            JobApplication? capturedApplication = null;

            this.automocker.GetMock<ICreateApplicationQuery>()
                .Setup(x => x.Execute(It.IsAny<JobApplication>()))
                .Callback<JobApplication>(a => capturedApplication = a)
                .Returns<JobApplication>(a => a);

            var sut = this.CreateTestSubject();

            // Act
            sut.CreateJobApplication(request);

            // Assert
            Assert.NotNull(capturedApplication);
            Assert.Single(capturedApplication.StatusHistory);
            Assert.Equal(ApplicationStatus.Applied, capturedApplication.StatusHistory[0].Status);
        }

        [Fact]
        public void CreateJobApplication_ReturnsResultFromQuery()
        {
            // Arrange
            var request = new CreateApplication
            {
                UserId = 1, 
                Company = "Google", 
                Role = "Senior Engineer"
            };
            
            var expectedApplication = new JobApplication
            {
                Id = 42, 
                UserId = 1, 
                Company = "Google", 
                Role = "Senior Engineer"
            };

            this.automocker.GetMock<ICreateApplicationQuery>()
                .Setup(x => x.Execute(It.IsAny<JobApplication>()))
                .Returns(expectedApplication);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.CreateJobApplication(request);

            // Assert
            Assert.Equal(42, result.Id);
            Assert.Equal("Google", result.Company);
        }
    }
}