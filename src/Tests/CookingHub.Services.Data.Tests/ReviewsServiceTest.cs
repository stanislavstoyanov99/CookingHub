namespace CookingHub.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using CookingHub.Data;
    using CookingHub.Data.Models;
    using CookingHub.Data.Models.Enumerations;
    using CookingHub.Data.Repositories;
    using CookingHub.Models.ViewModels.Recipes;
    using CookingHub.Models.ViewModels.Reviews;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class ReviewsServiceTest : IAsyncDisposable
    {
        private readonly IReviewsService reviewService;
        private EfDeletableEntityRepository<Review> reviewsRepository;
        private EfDeletableEntityRepository<Recipe> recipesRepository;
        private EfDeletableEntityRepository<Category> categoriesRepository;
        private EfDeletableEntityRepository<CookingHubUser> usersRepository;
        private SqliteConnection connection;

        private Review firstReview;
        private Recipe firstRecipe;
        private Category firstCategory;
        private CookingHubUser cookingHubUser;

        public ReviewsServiceTest()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.reviewService = new ReviewsService(this.reviewsRepository, this.recipesRepository);
        }

        public async ValueTask DisposeAsync()
        {
            await this.connection.CloseAsync();
            await this.connection.DisposeAsync();
        }

        [Fact]
        public async Task CheckIfReviewGetAllWorks()
        {
            this.SeedDatabase();

            var expectedCount = await this.reviewsRepository
                .All()
                .Where(r => r.RecipeId == this.firstRecipe.Id)
                .CountAsync();

            var result = await this.reviewService.GetAll<ReviewDetailsViewModel>(this.firstRecipe.Id);

            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public async Task CheckIfReviewGetAllThrowsException()
        {
            var exception = await Assert.ThrowsAsync<NullReferenceException>(
                async () => await this.reviewService.GetAll<ReviewDetailsViewModel>(3));
            Assert.Equal(ExceptionMessages.ReviewsNotFound, exception.Message);
        }

        [Fact]
        public async Task CheckIfReviewsCreateAsyncThrowsException()
        {
            var review = new CreateReviewInputModel()
            {
                Title = this.firstReview.Title,
                Rate = this.firstReview.Rate,
                Content = this.firstReview.Description,
                RecipeId = 17,
                UserId = "12",
            };
        }

        private void InitializeDatabaseAndRepositories()
        {
            this.connection = new SqliteConnection("DataSource=:memory:");
            this.connection.Open();
            var options = new DbContextOptionsBuilder<CookingHubDbContext>().UseSqlite(this.connection);
            var dbContext = new CookingHubDbContext(options.Options);

            dbContext.Database.EnsureCreated();

            this.usersRepository = new EfDeletableEntityRepository<CookingHubUser>(dbContext);
            this.categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            this.reviewsRepository = new EfDeletableEntityRepository<Review>(dbContext);
            this.recipesRepository = new EfDeletableEntityRepository<Recipe>(dbContext);
        }

        private void InitializeFields()
        {
            this.cookingHubUser = new CookingHubUser
            {
                Id = "1",
                FullName = "Peter Petrov",
                UserName = "Test user 1",
                Gender = Gender.Male,
            };

            this.firstCategory = new Category
            {
                Id = 1,
                Name = "Vegetables",
                Description = "Test description",
            };

            this.firstReview = new Review
            {
                Title = "Бива",
                Description = "Test description",
                Rate = 5,
                RecipeId = 1,
                UserId = "1",
            };

            this.firstRecipe = new Recipe
            {
                Name = "Test recipe name",
                Description = "Test description name",
                Ingredients = "Test ingredients here",
                Rate = 3,
                PreparationTime = 5,
                CookingTime = 3,
                PortionsNumber = 3,
                Difficulty = Difficulty.Easy,
                ImagePath = "Test image path",
                CategoryId = 1,
                UserId = "1",
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedUsers();
            await this.SeedCategories();
            await this.SeedRecipes();
            await this.SeedReviews();
        }

        private async Task SeedRecipes()
        {
            await this.recipesRepository.AddAsync(this.firstRecipe);
            await this.recipesRepository.SaveChangesAsync();
        }

        private async Task SeedReviews()
        {
            await this.reviewsRepository.AddAsync(this.firstReview);
            await this.reviewsRepository.SaveChangesAsync();
        }

        private async Task SeedUsers()
        {
            await this.usersRepository.AddAsync(this.cookingHubUser);
            await this.usersRepository.SaveChangesAsync();
        }

        private async Task SeedCategories()
        {
            await this.categoriesRepository.AddAsync(this.firstCategory);
            await this.categoriesRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CookingHub.Models.ViewModels"));
    }
}
