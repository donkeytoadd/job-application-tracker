namespace job_application_tracker.Tests.Data.Queries
{
    using job_application_tracker.Data;
    using job_application_tracker.Data.Entities;
    using job_application_tracker.Data.Enums;
    using job_application_tracker.Data.Queries;
    using Microsoft.EntityFrameworkCore;
    using Moq;

    public class GetApplicationsByUserIdQueryTests : TestBase<GetApplicationsByUserIdQuery>
    {
        private DbContextOptions<ApplicationDbContext> CreateOptions() =>
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        private void Seed(DbContextOptions<ApplicationDbContext> options, IEnumerable<JobApplication> applications)
        {
            using var context = new ApplicationDbContext(options);
            context.JobApplication.AddRange(applications);
            context.SaveChanges();
        }

        [Fact]
        public void Execute_ReturnsOnlyApplicationsForSpecifiedUser()
        {
            // Arrange
            var options = CreateOptions();
            Seed(options, new[]
            {
                new JobApplication
                {
                    UserId = 1, 
                    Company = "Google", 
                    Role = "Engineer"
                },
                new JobApplication
                {
                    UserId = 1, 
                    Company = "Microsoft", 
                    Role = "Developer"
                },
                new JobApplication
                {
                    UserId = 2, 
                    Company = "Apple", 
                    Role = "Engineer"
                }
            });

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute(userId: 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, a => Assert.Equal(1, a.UserId));
        }

        [Fact]
        public void Execute_ReturnsApplicationsOrderedByIdDescending()
        {
            // Arrange
            var options = CreateOptions();
            Seed(options, new[]
            {
                new JobApplication
                {
                    UserId = 1, 
                    Company = "Google", 
                    Role = "Engineer"
                },
                new JobApplication
                {
                    UserId = 1, 
                    Company = "Microsoft", 
                    Role = "Developer"
                },
                new JobApplication
                {
                    UserId = 1, 
                    Company = "Stripe", 
                    Role = "Backend Engineer"
                }
            });

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute(userId: 1);

            // Assert
            Assert.Equal("Stripe", result[0].Company);
            Assert.Equal("Microsoft", result[1].Company);
            Assert.Equal("Google", result[2].Company);
        }

        [Fact]
        public void Execute_IncludesStatusHistoryForEachApplication()
        {
            // Arrange
            var options = CreateOptions();

            using (var seedContext = new ApplicationDbContext(options))
            {
                seedContext.JobApplication.Add(new JobApplication
                {
                    UserId = 1,
                    Company = "Google",
                    Role = "Engineer",
                    StatusHistory = new List<ApplicationStatusHistory>
                    {
                        new()
                        {
                            Status = ApplicationStatus.Applied
                        },
                        new()
                        {
                            Status = ApplicationStatus.Screening
                        }
                    }
                });
                seedContext.SaveChanges();
            }

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute(userId: 1);

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result[0].StatusHistory.Count);
            Assert.Contains(result[0].StatusHistory, h => h.Status == ApplicationStatus.Applied);
            Assert.Contains(result[0].StatusHistory, h => h.Status == ApplicationStatus.Screening);
        }

        [Fact]
        public void Execute_ReturnsEmptyListForUserWithNoApplications()
        {
            // Arrange
            var options = CreateOptions();

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute(userId: 99);

            // Assert
            Assert.Empty(result);
        }
    }
}