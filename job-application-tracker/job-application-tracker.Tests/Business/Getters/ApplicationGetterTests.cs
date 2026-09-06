namespace job_application_tracker.Tests.Business.Getters
{
    using job_application_tracker.Business.Getters;
    using job_application_tracker.Data.Entities;
    using job_application_tracker.Data.Queries.Interfaces;
    using Moq;

    public class ApplicationGetterTests : TestBase<ApplicationGetter>
    {
        [Fact]
        public void GetApplicationsByUserId_ReturnsApplicationsFromQuery()
        {
            // Arrange
            var userId = 1;
            var expectedApplications = new List<JobApplication>
            {
                new() { Id = 1, UserId = userId, Company = "Google", Role = "Senior Engineer" },
                new() { Id = 2, UserId = userId, Company = "Microsoft", Role = "Software Engineer II" }
            };

            this.automocker.GetMock<IGetApplicationsByUserIdQuery>()
                .Setup(x => x.Execute(userId))
                .Returns(expectedApplications);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.GetApplicationsByUserId(userId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Google", result[0].Company);
            Assert.Equal("Microsoft", result[1].Company);
        }

        [Fact]
        public void GetApplicationsByUserId_ReturnsEmptyListWhenNoneFound()
        {
            // Arrange
            var userId = 99;

            this.automocker.GetMock<IGetApplicationsByUserIdQuery>()
                .Setup(x => x.Execute(userId))
                .Returns(new List<JobApplication>());

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.GetApplicationsByUserId(userId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetApplicationsByUserId_CallsQueryWithCorrectUserId()
        {
            // Arrange
            var userId = 5;

            this.automocker.GetMock<IGetApplicationsByUserIdQuery>()
                .Setup(x => x.Execute(userId))
                .Returns(new List<JobApplication>());

            var sut = this.CreateTestSubject();

            // Act
            sut.GetApplicationsByUserId(userId);

            // Assert
            this.automocker.GetMock<IGetApplicationsByUserIdQuery>()
                .Verify(x => x.Execute(userId), Times.Once);
        }
    }
}