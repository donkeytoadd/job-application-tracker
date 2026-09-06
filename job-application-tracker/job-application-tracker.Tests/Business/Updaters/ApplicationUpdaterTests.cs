namespace job_application_tracker.Tests.Business.Updaters
{
    using job_application_tracker.Business.Updaters;
    using job_application_tracker.Data.Entities;
    using job_application_tracker.Data.Enums;
    using job_application_tracker.Data.Queries.Interfaces;
    using Moq;

    public class ApplicationUpdaterTests : TestBase<ApplicationUpdater>
    {
        [Fact]
        public void UpdateApplicationStatus_ReturnsUpdatedApplicationFromQuery()
        {
            // Arrange
            var applicationId = 1;
            var newStatus = ApplicationStatus.Interview;

            var expectedApplication = new JobApplication
            {
                Id = applicationId,
                UserId = 1,
                Company = "Google",
                Role = "Senior Engineer",
                Status = newStatus
            };

            this.automocker.GetMock<IUpdateApplicationStatusQuery>()
                .Setup(x => x.Execute(applicationId, newStatus))
                .Returns(expectedApplication);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.UpdateApplicationStatus(applicationId, newStatus);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newStatus, result.Status);
            Assert.Equal("Google", result.Company);
        }

        [Fact]
        public void UpdateApplicationStatus_ReturnsNullWhenApplicationNotFound()
        {
            // Arrange
            var applicationId = 99;
            var newStatus = ApplicationStatus.Screening;

            this.automocker.GetMock<IUpdateApplicationStatusQuery>()
                .Setup(x => x.Execute(applicationId, newStatus))
                .Returns((JobApplication?)null);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.UpdateApplicationStatus(applicationId, newStatus);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateApplicationStatus_CallsQueryWithCorrectArguments()
        {
            // Arrange
            var applicationId = 5;
            var newStatus = ApplicationStatus.Offer;

            this.automocker.GetMock<IUpdateApplicationStatusQuery>()
                .Setup(x => x.Execute(applicationId, newStatus))
                .Returns((JobApplication?)null);

            var sut = this.CreateTestSubject();

            // Act
            sut.UpdateApplicationStatus(applicationId, newStatus);

            // Assert
            this.automocker.GetMock<IUpdateApplicationStatusQuery>()
                .Verify(x => x.Execute(applicationId, newStatus), Times.Once);
        }
    }
}