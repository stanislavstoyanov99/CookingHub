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
    using CookingHub.Models.InputModels.AdministratorInputModels.Recipes;
    using CookingHub.Models.ViewModels.Recipes;
    using CookingHub.Models.ViewModels.Reviews;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class RecipesServiceTest : IAsyncDisposable
    {
        private readonly IRecipesService recipesService;
        private readonly ICloudinaryService cloudinaryService;
        private EfDeletableEntityRepository<Recipe> recipesRepository;
        private EfDeletableEntityRepository<Category> categoriesRepository;
        private EfDeletableEntityRepository<CookingHubUser> usersRepository;
        private SqliteConnection connection;

        private Recipe firstRecipe;
        private Category firstCategory;
        private CookingHubUser cookingHubUser;

        public RecipesServiceTest()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.recipesService = new RecipesService(this.recipesRepository, this.categoriesRepository, this.cloudinaryService);
        }

        public async ValueTask DisposeAsync()
        {
            await this.connection.CloseAsync();
            await this.connection.DisposeAsync();
        }

        [Fact]
        public async Task TestAddingRecipe()
        {
            await this.SeedCategories();
            var model = new RecipeCreateInputModel
            {
                Name = this.firstRecipe.Name,
                Description = this.firstRecipe.Description,
                Ingredients = this.firstRecipe.Ingredients,
                PreparationTime = this.firstRecipe.PreparationTime,
                CookingTime = this.firstRecipe.CookingTime,
                PortionsNumber = this.firstRecipe.PortionsNumber,
                Difficulty = "Easy",
                ImagePath = this.firstRecipe.ImagePath,
                CategoryId = 1,
            };

           // await this.recipesService.CreateAsync(model, "1");
           // var count = await this.categoriesRepository.All().CountAsync();

           // Assert.Equal(1, count);
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

            this.firstRecipe = new Recipe
            {
                Name = "Test recipe name",
                Description = "Test description name which has to have at least 50 sybmols for...reasons!Anyway Test description.",
                Ingredients = "Test ingredients here",
                Rate = 3,
                PreparationTime = 5,
                CookingTime = 3,
                PortionsNumber = 3,
                Difficulty = Difficulty.Easy,
                ImagePath = "Test image path",
                CategoryId = 1,
                
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedUsers();
            await this.SeedCategories();
            await this.SeedRecipes();
        }

        private async Task SeedRecipes()
        {
            await this.recipesRepository.AddAsync(this.firstRecipe);
            await this.recipesRepository.SaveChangesAsync();
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
