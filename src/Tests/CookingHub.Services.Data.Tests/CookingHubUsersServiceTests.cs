namespace CookingHub.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CookingHub.Data;
    using CookingHub.Data.Models;
    using CookingHub.Data.Models.Enumerations;
    using CookingHub.Data.Repositories;
    using CookingHub.Models.ViewModels.CookingHubUsers;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class CookingHubUsersServiceTests : IAsyncDisposable
    {
        private readonly ICookingHubUsersService cookingHubUsersService;
        private EfDeletableEntityRepository<CookingHubUser> cookingHubUsersRepository;
        private SqliteConnection connection;

        private CookingHubUser firstCookingHubUser;

        public CookingHubUsersServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.cookingHubUsersService = new CookingHubUsersService(this.cookingHubUsersRepository);
        }

        [Fact]
        public async Task CheckIfBanByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            await this.cookingHubUsersService.BanByIdAsync(this.firstCookingHubUser.Id);

            var count = await this.cookingHubUsersRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfBanByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.cookingHubUsersService.BanByIdAsync("3"));

            Assert.Equal(string.Format(ExceptionMessages.CookingHubUserNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfUnbanByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            await this.cookingHubUsersService.UnbanByIdAsync(this.firstCookingHubUser.Id);

            var count = await this.cookingHubUsersRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfUnbanByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.cookingHubUsersService.UnbanByIdAsync("2"));

            Assert.Equal(string.Format(ExceptionMessages.CookingHubUserNotFound, 2), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllCookingHubUsersAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.cookingHubUsersService.GetAllCookingHubUsersAsync<CookingHubUserDetailsViewModel>();

            var count = result.Count();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new CookingHubUserDetailsViewModel
            {
                Id = this.firstCookingHubUser.Id,
                Username = this.firstCookingHubUser.UserName,
                FullName = this.firstCookingHubUser.FullName,
                CreatedOn = this.firstCookingHubUser.CreatedOn,
                isDeleted = this.firstCookingHubUser.IsDeleted,
                Gender = this.firstCookingHubUser.Gender,
            };

            var viewModel = await this.cookingHubUsersService
                .GetViewModelByIdAsync<CookingHubUserDetailsViewModel>(this.firstCookingHubUser.Id);

            var expectedObj = JsonConvert.SerializeObject(expectedModel);
            var actualResultObj = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(expectedObj, actualResultObj);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(
                async () => await this.cookingHubUsersService.GetViewModelByIdAsync<CookingHubUserDetailsViewModel>("3"));

            Assert.Equal(string.Format(ExceptionMessages.CookingHubUserNotFound, "3"), exception.Message);
        }

        public async ValueTask DisposeAsync()
        {
            await this.connection.CloseAsync();
            await this.connection.DisposeAsync();
        }

        private void InitializeDatabaseAndRepositories()
        {
            this.connection = new SqliteConnection("DataSource=:memory:");
            this.connection.Open();
            var options = new DbContextOptionsBuilder<CookingHubDbContext>().UseSqlite(this.connection);
            var dbContext = new CookingHubDbContext(options.Options);

            dbContext.Database.EnsureCreated();

            this.cookingHubUsersRepository = new EfDeletableEntityRepository<CookingHubUser>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstCookingHubUser = new CookingHubUser
            {
                Id = "1",
                FullName = "Kiril Petrov",
                UserName = "Kiril789",
                Gender = Gender.Male,
                CreatedOn = DateTime.Parse("2020-02-10"),
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedUsers();
        }

        private async Task SeedUsers()
        {
            await this.cookingHubUsersRepository.AddAsync(this.firstCookingHubUser);

            await this.cookingHubUsersRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CookingHub.Models.ViewModels"));
    }
}
