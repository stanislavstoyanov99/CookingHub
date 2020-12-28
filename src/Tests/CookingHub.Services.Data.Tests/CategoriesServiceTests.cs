namespace CookingHub.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CookingHub.Data;
    using CookingHub.Data.Models;
    using CookingHub.Data.Repositories;
    using CookingHub.Models.InputModels.AdministratorInputModels.Categories;
    using CookingHub.Models.ViewModels.Categories;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class CategoriesServiceTests : IAsyncDisposable
    {
        private readonly ICategoriesService categoriesService;
        private EfDeletableEntityRepository<Category> categoriesRepository;
        private SqliteConnection connection;

        private Category firstCategory;

        public CategoriesServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.categoriesService = new CategoriesService(this.categoriesRepository);
        }

        [Fact]
        public async Task TestAddingCategory()
        {
            var model = new CategoryCreateInputModel
            {
                Name = "Wips",
                Description = "Some description...",
            };

            await this.categoriesService.CreateAsync(model);
            var count = await this.categoriesRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfCategoryProperties()
        {
            var model = new CategoryCreateInputModel
            {
                Name = "Three",
                Description = "Some description...",
            };

            await this.categoriesService.CreateAsync(model);

            var cateogry = await this.categoriesRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Three", cateogry.Name);
            Assert.Equal("Some description...", cateogry.Description);
        }

        [Fact]
        public async Task CheckIfAddingCategoryThrowsArgumentException()
        {
            this.SeedDatabase();

            var category = new CategoryCreateInputModel
            {
                Name = this.firstCategory.Name,
                Description = this.firstCategory.Description,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.categoriesService.CreateAsync(category));

            Assert.Equal(string.Format(ExceptionMessages.CategoryAlreadyExists, category.Name), exception.Message);
        }

        [Fact]
        public async Task CheckIfAddingCategoryReturnsViewModel()
        {
            var brand = new CategoryCreateInputModel
            {
                Name = "Filters",
                Description = "Different filters for wips",
            };

            var viewModel = await this.categoriesService.CreateAsync(brand);
            var dbEntry = await this.categoriesRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel.Id);
            Assert.Equal(dbEntry.Name, viewModel.Name);
            Assert.Equal(dbEntry.Description, viewModel.Description);
        }

        [Fact]
        public async Task CheckIfDeletingCategoryWorksCorrectly()
        {
            this.SeedDatabase();

            await this.categoriesService.DeleteByIdAsync(this.firstCategory.Id);

            var count = await this.categoriesRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingCategoryReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.categoriesService.DeleteByIdAsync(3));

            Assert.Equal(string.Format(ExceptionMessages.CategoryNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfEditingCategoryWorksCorrectly()
        {
            this.SeedDatabase();

            var categoryEditViewModel = new CategoryEditViewModel
            {
                Id = this.firstCategory.Id,
                Name = "Wips",
                Description = "Information about wips",
            };

            await this.categoriesService.EditAsync(categoryEditViewModel);

            Assert.Equal(categoryEditViewModel.Id, this.firstCategory.Id);
            Assert.Equal(categoryEditViewModel.Name, this.firstCategory.Name);
            Assert.Equal(categoryEditViewModel.Description, this.firstCategory.Description);
        }

        [Fact]
        public async Task CheckIfEditingCategoryReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var categoryEditViewModel = new CategoryEditViewModel
            {
                Id = 3,
                Name = "Three",
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.categoriesService.EditAsync(categoryEditViewModel));

            Assert.Equal(string.Format(ExceptionMessages.CategoryNotFound, categoryEditViewModel.Id), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllCategoriesAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.categoriesService.GetAllCategoriesAsync<CategoryDetailsViewModel>();

            var count = result.Count();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetCategoryAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.categoriesService.GetCategoryAsync<CategoryDetailsViewModel>(this.firstCategory.Name);

            Assert.Equal(this.firstCategory.Name, result.Name);
            Assert.Equal(this.firstCategory.Description, result.Description);
        }

        [Fact]
        public async Task CheckIfGetCategoryViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new CategoryDetailsViewModel
            {
                Id = this.firstCategory.Id,
                Name = this.firstCategory.Name,
                Description = this.firstCategory.Description,
            };

            var viewModel = await this.categoriesService.GetViewModelByIdAsync<CategoryDetailsViewModel>(this.firstCategory.Id);

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
                async () => await this.categoriesService.GetViewModelByIdAsync<CategoryDetailsViewModel>(3));

            Assert.Equal(string.Format(ExceptionMessages.CategoryNotFound, 3), exception.Message);
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

            this.categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstCategory = new Category
            {
                Name = "Fruits",
                Description = "Different Fruits deserts from world class chefs",
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedCategories();
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
