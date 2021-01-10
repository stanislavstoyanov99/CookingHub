namespace CookingHub.Services.Data.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using CookingHub.Data;
    using CookingHub.Data.Models;
    using CookingHub.Data.Models.Enumerations;
    using CookingHub.Data.Repositories;
    using CookingHub.Models.InputModels.AdministratorInputModels.Recipes;
    using CookingHub.Models.ViewModels.Recipes;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class RecipesServiceTests : IAsyncDisposable, IClassFixture<Configuration>
    {
        private readonly IRecipesService recipesService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly Cloudinary cloudinary;
        private EfDeletableEntityRepository<Recipe> recipesRepository;
        private EfDeletableEntityRepository<Category> categoriesRepository;
        private EfDeletableEntityRepository<CookingHubUser> usersRepository;
        private SqliteConnection connection;

        private Recipe firstRecipe;
        private Category firstCategory;
        private CookingHubUser cookingHubUser;

        public RecipesServiceTests(Configuration configuration)
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            Account account = new Account(
              configuration.ConfigurationRoot["Cloudinary:AppName"],
              configuration.ConfigurationRoot["Cloudinary:AppKey"],
              configuration.ConfigurationRoot["Cloudinary:AppSecret"]);

            this.cloudinary = new Cloudinary(account);
            this.cloudinaryService = new CloudinaryService(this.cloudinary);
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
            await this.SeedUsers();
            await this.SeedCategories();

            var path = "Test.jpg";

            RecipeDetailsViewModel recipeDetailsViewModel;
            using (var img = File.OpenRead(path))
            {
                var testImage = new FormFile(img, 0, img.Length, "Test.jpg", img.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };

                var model = new RecipeCreateInputModel
                {
                    Name = this.firstRecipe.Name,
                    Description = this.firstRecipe.Description,
                    Ingredients = this.firstRecipe.Ingredients,
                    PreparationTime = this.firstRecipe.PreparationTime,
                    CookingTime = this.firstRecipe.CookingTime,
                    PortionsNumber = this.firstRecipe.PortionsNumber,
                    Difficulty = "Easy",
                    Image = testImage,
                    CategoryId = 1,
                };
                recipeDetailsViewModel = await this.recipesService.CreateAsync(model, "1");
            }

            await this.cloudinaryService.DeleteImage(this.cloudinary, recipeDetailsViewModel.Name + Suffixes.RecipeSuffix);
            var count = await this.categoriesRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task TestAddingRecipeWithInvalidDifficulty()
        {
            await this.SeedUsers();
            await this.SeedCategories();

            var path = "Test.jpg";

            RecipeCreateInputModel model;
            using (var img = File.OpenRead(path))
            {
                var testImage = new FormFile(img, 0, img.Length, "Test.jpg", img.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };

                model = new RecipeCreateInputModel
                {
                    Name = this.firstRecipe.Name,
                    Description = this.firstRecipe.Description,
                    Ingredients = this.firstRecipe.Ingredients,
                    PreparationTime = this.firstRecipe.PreparationTime,
                    CookingTime = this.firstRecipe.CookingTime,
                    PortionsNumber = this.firstRecipe.PortionsNumber,
                    Difficulty = "Invalid difficulty",
                    Image = testImage,
                    CategoryId = 1,
                };
            }

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.recipesService.CreateAsync(model, this.cookingHubUser.Id));

            Assert.Equal(string.Format(ExceptionMessages.DifficultyInvalidType, model.Difficulty), exception.Message);
        }

        [Fact]
        public async Task TestAddingAlreadyExistingRecipe()
        {
            this.SeedDatabase();

            var path = "Test.jpg";

            RecipeCreateInputModel model;
            using (var img = File.OpenRead(path))
            {
                var testImage = new FormFile(img, 0, img.Length, "Test.jpg", img.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };

                model = new RecipeCreateInputModel
                {
                    Name = this.firstRecipe.Name,
                    Description = this.firstRecipe.Description,
                    Ingredients = this.firstRecipe.Ingredients,
                    PreparationTime = this.firstRecipe.PreparationTime,
                    CookingTime = this.firstRecipe.CookingTime,
                    PortionsNumber = this.firstRecipe.PortionsNumber,
                    Difficulty = "Easy",
                    Image = testImage,
                    CategoryId = 1,
                };
            }

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.recipesService.CreateAsync(model, this.cookingHubUser.Id));

            Assert.Equal(string.Format(ExceptionMessages.RecipeAlreadyExists, model.Name), exception.Message);
        }

        [Fact]
        public async Task TestAddingRecipeWithMissingCategory()
        {
            await this.SeedUsers();

            var path = "Test.jpg";

            RecipeCreateInputModel model;
            using (var img = File.OpenRead(path))
            {
                var testImage = new FormFile(img, 0, img.Length, "Test.jpg", img.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };

                model = new RecipeCreateInputModel
                {
                    Name = this.firstRecipe.Name,
                    Description = this.firstRecipe.Description,
                    Ingredients = this.firstRecipe.Ingredients,
                    PreparationTime = this.firstRecipe.PreparationTime,
                    CookingTime = this.firstRecipe.CookingTime,
                    PortionsNumber = this.firstRecipe.PortionsNumber,
                    Difficulty = "Easy",
                    Image = testImage,
                    CategoryId = 1,
                };
            }

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.recipesService.CreateAsync(model, this.cookingHubUser.Id));

            Assert.Equal(string.Format(ExceptionMessages.CategoryNotFound, model.CategoryId), exception.Message);
        }

        [Fact]
        public async Task TestRecipeDeleteById()
        {
            this.SeedDatabase();

            await this.recipesService.DeleteByIdAsync(1);
            var result = await this.recipesRepository.All().CountAsync();

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task TestRecipeDeleteByIdThrowsException()
        {
            this.SeedDatabase();

            var exception = await Assert.ThrowsAsync<NullReferenceException>(
                  async () => await this.recipesService.DeleteByIdAsync(3));

            Assert.Equal(string.Format(ExceptionMessages.RecipeNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task TestIfRecipeEditAsyncWorks()
        {
            this.SeedDatabase();

            var path = "Test.jpg";
            using (var img = File.OpenRead(path))
            {
                var testImage = new FormFile(img, 0, img.Length, "Test.jpg", img.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };

                var model = new RecipeEditViewModel
                {
                    Id = 1,
                    Name = this.firstRecipe.Name,
                    Description = this.firstRecipe.Description,
                    Ingredients = this.firstRecipe.Ingredients,
                    PreparationTime = this.firstRecipe.PreparationTime,
                    CookingTime = this.firstRecipe.CookingTime,
                    PortionsNumber = this.firstRecipe.PortionsNumber,
                    Difficulty = "Medium",
                    Image = testImage,
                    CategoryId = 1,
                };

                await this.recipesService.EditAsync(model);
            }

            var count = await this.recipesRepository.All().CountAsync();
            var result = await this.recipesRepository
                .All()
                .Where(x => x.Id == 1)
                .Select(o => o.Difficulty.ToString())
                .FirstOrDefaultAsync();

            Assert.Equal(1, count);
            Assert.Equal("Medium", result);
        }

        [Fact]
        public async Task TestEditingRecipeWithInvalidDifficulty()
        {
            await this.SeedUsers();
            await this.SeedCategories();

            var path = "Test.jpg";

            RecipeEditViewModel model;
            using (var img = File.OpenRead(path))
            {
                var testImage = new FormFile(img, 0, img.Length, "Test.jpg", img.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };

                model = new RecipeEditViewModel
                {
                    Name = "Changed name",
                    Description = "Changed description",
                    Ingredients = "Changed ingredients",
                    PreparationTime = 20,
                    CookingTime = 10,
                    PortionsNumber = 3,
                    Difficulty = "Invalid difficulty",
                    Image = testImage,
                    CategoryId = 1,
                };
            }

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.recipesService.EditAsync(model));

            Assert.Equal(string.Format(ExceptionMessages.DifficultyInvalidType, model.Difficulty), exception.Message);
        }

        [Fact]
        public async Task TestEditingRecipeWithMissingRecipe()
        {
            await this.SeedUsers();
            await this.SeedCategories();

            var path = "Test.jpg";

            RecipeEditViewModel model;
            using (var img = File.OpenRead(path))
            {
                var testImage = new FormFile(img, 0, img.Length, "Test.jpg", img.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };

                model = new RecipeEditViewModel
                {
                    Id = 1,
                    Name = this.firstRecipe.Name,
                    Description = this.firstRecipe.Description,
                    Ingredients = this.firstRecipe.Ingredients,
                    PreparationTime = this.firstRecipe.PreparationTime,
                    CookingTime = this.firstRecipe.CookingTime,
                    PortionsNumber = this.firstRecipe.PortionsNumber,
                    Difficulty = "Easy",
                    Image = testImage,
                    CategoryId = 1,
                };
            }

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.recipesService.EditAsync(model));

            Assert.Equal(string.Format(ExceptionMessages.RecipeNotFound, model.Id), exception.Message);
        }

        [Fact]
        public async Task TestIfRecipeServiceGetAllWorks()
        {
            this.SeedDatabase();

            var model = await this.recipesService.GetAllRecipesAsync<RecipeDetailsViewModel>();

            Assert.Single(model);
        }

        [Fact]
        public void TestIfGetAllRecipesByFilterAsQueryeableWorks()
        {
            this.SeedDatabase();

            var model = this.recipesService.GetAllRecipesByFilterAsQueryeable<RecipeDetailsViewModel>("Vegetables");

            Assert.NotNull(model);
        }

        [Fact]
        public async Task CheckIfGetAllRecipesByFilterAsQueryeableWorksCorrectlyWithoutCategoryName()
        {
            this.SeedDatabase();

            var result = this.recipesService.GetAllRecipesByFilterAsQueryeable<RecipeDetailsViewModel>();
            var recipe = await result.FirstAsync();

            var count = await result.CountAsync();

            Assert.Equal(1, count);
            Assert.Equal(this.firstRecipe.Name, recipe.Name);
        }

        [Fact]
        public async Task TestIfGetTopRecipesAsyncWorks()
        {
            this.SeedDatabase();

            var model = await this.recipesService.GetTopRecipesAsync<RecipeDetailsViewModel>(3);

            Assert.Single(model);
        }

        [Fact]
        public async Task TestIfGetRecipeByIdThrowsExeption()
        {
            this.SeedDatabase();

            var exception = await Assert.ThrowsAsync<NullReferenceException>(
                  async () => await this.recipesService.GetViewModelByIdAsync<RecipeDetailsViewModel>(3));

            Assert.NotNull(exception);
            Assert.Equal(string.Format(ExceptionMessages.RecipeNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task TestIfGetAllRecipesByUserIdWorks()
        {
            this.SeedDatabase();

            var model = await this.recipesService
                .GetAllRecipesByUserId<RecipeDetailsViewModel>(this.cookingHubUser.Id);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task TestIfGetRecipeAsyncWorks()
        {
            this.SeedDatabase();

            var model = await this.recipesService.GetRecipeAsync<RecipeDetailsViewModel>(this.firstRecipe.Name);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task TestIfGetRecipeAsyncThrowsExceptionForInvalidInput()
        {
            this.SeedDatabase();

            var exception = await Assert.ThrowsAsync<NullReferenceException>(
                  async () => await this.recipesService.GetRecipeAsync<RecipeDetailsViewModel>("Vino"));

            Assert.NotNull(exception);
            Assert.Equal(string.Format(ExceptionMessages.RecipeNameNotFound, "Vino"), exception.Message);
        }

        [Fact]
        public void TestIfGetAllRecipesAsQueryeableWorks()
        {
            this.SeedDatabase();

            var model = this.recipesService.GetAllRecipesAsQueryeable<RecipeDetailsViewModel>();

            Assert.NotNull(model);
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
                UserId = "1",
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
