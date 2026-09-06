namespace job_application_tracker.Tests.Data.Queries
{
    using job_application_tracker.Data;
    using job_application_tracker.Data.Entities;
    using job_application_tracker.Data.Queries;
    using Microsoft.EntityFrameworkCore;
    using Moq;

    public class CreateUserQueryTests : TestBase<CreateUserQuery>
    {
        private DbContextOptions<ApplicationDbContext> CreateOptions() =>
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        [Fact]
        public void Execute_ReturnsCreatedUser()
        {
            // Arrange
            var options = CreateOptions();

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var user = new User
            {
                Email = "jordan@example.com", 
                PasswordHash = "hashed_password"
            };
            
            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute(user);

            // Assert
            Assert.Equal("jordan@example.com", result.Email);
            Assert.Equal("hashed_password", result.PasswordHash);
        }
    }
}