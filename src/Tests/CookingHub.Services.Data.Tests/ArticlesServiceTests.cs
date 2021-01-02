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
    using CookingHub.Models.InputModels.AdministratorInputModels.Articles;
    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ArticlesServiceTests : IAsyncDisposable, IClassFixture<Configuration>
    {
        private readonly IArticlesService articlesService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly Cloudinary cloudinary;
        private EfDeletableEntityRepository<Article> articleRepisitory;
        private EfDeletableEntityRepository<Category> categoriesRepository;
        private EfDeletableEntityRepository<CookingHubUser> usersRepository;
        private SqliteConnection connection;

        private Article firstArticle;
        private Category firstCategory;
        private CookingHubUser cookingHubUser;

        public ArticlesServiceTests(Configuration configuration)
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
            this.articlesService = new ArticlesService(this.articleRepisitory, this.categoriesRepository, this.cloudinaryService);
        }

        public async ValueTask DisposeAsync()
        {
            await this.connection.CloseAsync();
            await this.connection.DisposeAsync();
        }

        [Fact]
        public async Task TestCreatingArticle()
        {
            await this.SeedUsers();
            await this.SeedCategories();

            var path = "Test.jpg";
            ArticleDetailsViewModel articleDetailsViewModel;

            using (var img = File.OpenRead(path))
            {
                var testImage = new FormFile(img, 0, img.Length, "Test.jpg", img.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };

                var model = new ArticleCreateInputModel
                {
                    Title = this.firstArticle.Title,
                    Description = this.firstArticle.Description,
                    Image = testImage,
                    CategoryId = 1,
                };
                articleDetailsViewModel = await this.articlesService.CreateAsync(model, "1");
            }

            await this.cloudinaryService.DeleteImage(this.cloudinary, articleDetailsViewModel.Title + Suffixes.RecipeSuffix);
            var count = await this.categoriesRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task TestAddingRecipeWithMissingCategory()
        {
            await this.SeedUsers();
            await this.SeedCategories();

            var path = "Test.jpg";
            ArticleCreateInputModel model;

            using (var img = File.OpenRead(path))
            {
                var testImage = new FormFile(img, 0, img.Length, "Test.jpg", img.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };

                model = new ArticleCreateInputModel
                {
                    Title = this.firstArticle.Title,
                    Description = this.firstArticle.Description,
                    Image = testImage,
                    CategoryId = 2,
                };
            }

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.articlesService.CreateAsync(model, this.cookingHubUser.Id));

            Assert.Equal(string.Format(ExceptionMessages.CategoryNotFound, model.CategoryId), exception.Message);
        }

        [Fact]
        public async Task TestAddingAlreadyExistingArticle()
        {
            await this.SeedDatabase();

            var path = "Test.jpg";
            ArticleCreateInputModel model;

            using (var img = File.OpenRead(path))
            {
                var testImage = new FormFile(img, 0, img.Length, "Test.jpg", img.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };

                model = new ArticleCreateInputModel
                {
                    Title = this.firstArticle.Title,
                    Description = this.firstArticle.Description,
                    Image = testImage,
                    CategoryId = 1,
                };
            }

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.articlesService.CreateAsync(model, this.cookingHubUser.Id));

            Assert.Equal(string.Format(ExceptionMessages.ArticleAlreadyExists, model.Title), exception.Message);
        }

        [Fact]
        public async Task TestGettingAllArticles()
        {
            await this.SeedDatabase();

            await this.articlesService.DeleteByIdAsync(1);
            var result = await this.articleRepisitory.All().CountAsync();

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task TestArticleDeleteByIdWorks()
        {
            await this.SeedDatabase();

            await this.articlesService.DeleteByIdAsync(1);

            var result1 = await this.articleRepisitory.All().CountAsync();
            var result2 = await this.articleRepisitory.All().Where(x => x.Id == 1).AnyAsync();

            Assert.Equal(0, result1);
            Assert.False(result2);
        }

        [Fact]
        public async Task TestArticleDeleteByIdThrowsException()
        {
            await this.SeedDatabase();

            var exception = await Assert.ThrowsAsync<NullReferenceException>(
                  async () => await this.articlesService.DeleteByIdAsync(3));

            Assert.Equal(string.Format(ExceptionMessages.ArticleNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task TestIfArticleEditAsyncWorks()
        {
            await this.SeedDatabase();

            var path = "Test.jpg";
            using (var img = File.OpenRead(path))
            {
                var testImage = new FormFile(img, 0, img.Length, "Test.jpg", img.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };

                var model = new ArticleEditViewModel
                {
                    Id = 1,
                    Title = "TestEditTitle",
                    Description = this.firstArticle.Description,
                    Image = testImage,
                    CategoryId = 1,
                };

                await this.articlesService.EditAsync(model, "1");
            }

            var count = await this.articleRepisitory.All().CountAsync();
            var result = await this.articleRepisitory
                .All()
                .Where(x => x.Id == 1)
                .Select(a => a.Title.ToString())
                .FirstOrDefaultAsync();

            Assert.Equal(1, count);
            Assert.Equal("TestEditTitle", result);
        }

        [Fact]
        public async Task TestEditingArticleWithMissingArticle()
        {
            await this.SeedUsers();
            await this.SeedCategories();

            var path = "Test.jpg";

            ArticleEditViewModel model;
            using (var img = File.OpenRead(path))
            {
                var testImage = new FormFile(img, 0, img.Length, "Test.jpg", img.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg",
                };

                model = new ArticleEditViewModel
                {
                    Id = 1,
                    Title = "TestEditTitle",
                    Description = this.firstArticle.Description,
                    Image = testImage,
                    CategoryId = 1,
                };
            }

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.articlesService.EditAsync(model, this.cookingHubUser.Id));

            Assert.Equal(string.Format(ExceptionMessages.ArticleNotFound, model.Id), exception.Message);
        }

        [Fact]
        public async Task TestIfArticlesServiceGetAllWorks()
        {
            await this.SeedDatabase();

            var model = await this.articlesService.GetAllArticlesAsync<ArticleDetailsViewModel>();

            Assert.Single(model);
        }

        [Fact]
        public async void TestIfGetAllArticlesByFilterAsQueryeableWorks()
        {
            await this.SeedDatabase();

            var model = this.articlesService
                .GetAllArticlesByCategoryNameAsQueryeable<ArticleDetailsViewModel>("Vegetables");

            Assert.NotNull(model);
        }

        [Fact]
        public async void TestIfGetAllArticlesAsQueryeableWorks()
        {
            await this.SeedDatabase();

            var articles = this.articlesService.GetAllArticlesAsQueryeable<ArticleDetailsViewModel>();
            var result = await articles.FirstOrDefaultAsync();

            Assert.Equal(1, await articles.CountAsync());
            Assert.Equal(this.firstArticle.Title, result.Title);
        }

        [Fact]
        public async Task TestIfGetRecentArticlesAsyncWorks()
        {
            await this.SeedDatabase();

            var model = await this.articlesService.GetRecentArticlesAsync<ArticleDetailsViewModel>(3);

            Assert.Single(model);
        }

        [Fact]
        public async Task TestIfGetArticleByIdThrowsExeption()
        {
            await this.SeedDatabase();

            var exception = await Assert.ThrowsAsync<NullReferenceException>(
                  async () => await this.articlesService.GetViewModelByIdAsync<ArticleDetailsViewModel>(3));

            Assert.NotNull(exception);
            Assert.Equal(string.Format(ExceptionMessages.ArticleNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task TestIfGetArticleAsyncWorks()
        {
            await this.SeedDatabase();

            var model = await this.articlesService.GetViewModelByIdAsync<ArticleDetailsViewModel>(1);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task TestIfGetArticleAsyncThrowsExceptionForInvalidInput()
        {
            await this.SeedDatabase();

            var exception = await Assert.ThrowsAsync<NullReferenceException>(
                  async () => await this.articlesService.GetViewModelByIdAsync<ArticleDetailsViewModel>(2));

            Assert.NotNull(exception);
            Assert.Equal(string.Format(ExceptionMessages.ArticleNotFound, 2), exception.Message);
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
            this.articleRepisitory = new EfDeletableEntityRepository<Article>(dbContext);
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

            this.firstArticle = new Article
            {
                Title = "Test Article",
                Description = "Test Description",
                ImagePath = "Test IMG path",
                CategoryId = 1,
                UserId = "1",
            };
        }

        private async Task SeedDatabase()
        {
            await this.SeedUsers();
            await this.SeedCategories();
            await this.SeedArticles();
        }

        private async Task SeedArticles()
        {
            await this.articleRepisitory.AddAsync(this.firstArticle);
            await this.articleRepisitory.SaveChangesAsync();
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
