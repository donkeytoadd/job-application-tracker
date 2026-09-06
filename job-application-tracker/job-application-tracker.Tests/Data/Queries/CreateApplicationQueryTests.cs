namespace job_application_tracker.Tests.Data.Queries
{
    using job_application_tracker.Data;
    using job_application_tracker.Data.Entities;
    using job_application_tracker.Data.Queries;
    using Microsoft.EntityFrameworkCore;
    using Moq;

    public class CreateApplicationQueryTests : TestBase<CreateApplicationQuery>
    {
        private DbContextOptions<ApplicationDbContext> CreateOptions() =>
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        [Fact]
        public void Execute_ReturnsCreatedApplication()
        {
            // Arrange
            var options = CreateOptions();

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var application = new JobApplication
            {
                UserId = 1, 
                Company = "Google", 
                Role = "Senior Engineer"
            };
            
            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute(application);

            // Assert
            Assert.Equal("Google", result.Company);
            Assert.Equal("Senior Engineer", result.Role);
            Assert.Equal(1, result.UserId);
        }
    }
}