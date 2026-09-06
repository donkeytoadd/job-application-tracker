namespace job_application_tracker.Tests.Data.Queries
{
    using job_application_tracker.Data;
    using job_application_tracker.Data.Entities;
    using job_application_tracker.Data.Queries;
    using Microsoft.EntityFrameworkCore;
    using Moq;

    public class GetUserByEmailQueryTests : TestBase<GetUserByEmailQuery>
    {
        private DbContextOptions<ApplicationDbContext> CreateOptions() =>
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        [Fact]
        public void Execute_ReturnsUserWithMatchingEmail()
        {
            // Arrange
            var options = CreateOptions();

            using (var seedContext = new ApplicationDbContext(options))
            {
                seedContext.User.Add(new User { Name = "Jordan Hanson", Email = "jordan@example.com", PasswordHash = "hash" });
                seedContext.SaveChanges();
            }

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute("jordan@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("jordan@example.com", result.Email);
            Assert.Equal("Jordan Hanson", result.Name);
        }

        [Fact]
        public void Execute_ReturnsNullWhenEmailNotFound()
        {
            // Arrange
            var options = CreateOptions();

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute("notfound@example.com");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_IsCaseSensitiveForEmail()
        {
            // Arrange
            var options = CreateOptions();

            using (var seedContext = new ApplicationDbContext(options))
            {
                seedContext.User.Add(new User { Name = "Jordan Hanson", Email = "jordan@example.com", PasswordHash = "hash" });
                seedContext.SaveChanges();
            }

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute("JORDAN@EXAMPLE.COM");

            // Assert — InMemory string comparison matches SQL Server's case-insensitive collation behaviour
            // This test documents current behaviour; update if collation is changed
            Assert.Null(result);
        }
    }
}