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
        private SqliteConnection connection;
        public Review firstReview;
        public Recipe firstRecipe;

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
            var expectedCount = await this.reviewsRepository.All().Where(r => r.RecipeId == this.firstRecipe.Id).CountAsync();

            var result = await this.reviewService.GetAll<ReviewDetailsViewModel>(this.firstRecipe.Id);

            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public async Task CheckIfReviewGetAllThrowsException()
        {
            var exception = await Assert.ThrowsAsync<NullReferenceException>( async () => await this.reviewService.GetAll<RecipeDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.ReviewsNotFound, 3), exception.Message);
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

            this.reviewsRepository = new EfDeletableEntityRepository<Review>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstReview = new Review
            {
                Id = 10,
                Title = "Бива",
                Description = " ",
                Rate = 5,

            };
            this.firstRecipe = new Recipe
            {
                Id = 17,
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedReview();
        }

        private async Task SeedReview()
        {
            await this.reviewsRepository.AddAsync(this.firstReview);

            await this.reviewsRepository.SaveChangesAsync();
        }
        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CookingHub.Models.ViewModels"));
    }
}
