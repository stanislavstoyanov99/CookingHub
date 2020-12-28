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
    using CookingHub.Models.ViewModels.ArticleComments;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ArticleCommentsServiceTests : IAsyncDisposable
    {
        private readonly IArticleCommentsService articleCommentsService;
        private EfDeletableEntityRepository<Article> articlesRepository;
        private EfDeletableEntityRepository<Category> categoriesRepository;
        private EfDeletableEntityRepository<ArticleComment> articleCommentsRepository;
        private EfDeletableEntityRepository<CookingHubUser> usersRepository;
        private SqliteConnection connection;

        private Article firstArticle;
        private Category firstCategory;
        private ArticleComment firstArticleComment;
        private CookingHubUser firstCookingHubUser;

        public ArticleCommentsServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.articleCommentsService = new ArticleCommentsService(this.articleCommentsRepository);
        }

        [Fact]
        public async Task CheckIfCreateAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var articleComment = new CreateArticleCommentInputModel
            {
                ArticleId = this.firstArticle.Id,
                Content = "I like this article.",
            };

            await this.articleCommentsService.CreateAsync(
                articleComment.ArticleId,
                this.firstCookingHubUser.Id,
                articleComment.Content);

            var count = await this.articleCommentsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfArticleCommentProperties()
        {
            this.SeedDatabase();

            var model = new CreateArticleCommentInputModel
            {
                ArticleId = this.firstArticle.Id,
                Content = "What's your opinion for the article?",
            };

            await this.articleCommentsService.CreateAsync(model.ArticleId, this.firstCookingHubUser.Id, model.Content);

            var articleComment = await this.articleCommentsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(model.ArticleId, articleComment.ArticleId);
            Assert.Equal("What's your opinion for the article?", articleComment.Content);
        }

        [Fact]
        public async Task CheckIfAddingArticleCommentThrowsArgumentException()
        {
            this.SeedDatabase();
            await this.SeedArticleComments();

            var articleComment = new CreateArticleCommentInputModel
            {
                ArticleId = this.firstArticle.Id,
                Content = this.firstArticleComment.Content,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async ()
                    => await this.articleCommentsService
                    .CreateAsync(articleComment.ArticleId, this.firstCookingHubUser.Id, articleComment.Content));

            Assert.Equal(
                string.Format(
                    ExceptionMessages.ArticleCommentAlreadyExists, articleComment.ArticleId, articleComment.Content), exception.Message);
        }

        [Fact]
        public async Task CheckIfIsInArticleIdReturnsTrue()
        {
            this.SeedDatabase();
            await this.SeedArticleComments();

            var articleCommentId = await this.articleCommentsRepository
                .All()
                .Select(x => x.ArticleId)
                .FirstOrDefaultAsync();

            var result = await this.articleCommentsService.IsInArticleId(articleCommentId, this.firstArticle.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfIsInArticleIdReturnsFalse()
        {
            this.SeedDatabase();
            await this.SeedArticleComments();

            var result = await this.articleCommentsService.IsInArticleId(3, this.firstArticle.Id);

            Assert.False(result);
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

            this.usersRepository = new EfDeletableEntityRepository<CookingHubUser>(dbContext);
            this.articlesRepository = new EfDeletableEntityRepository<Article>(dbContext);
            this.articleCommentsRepository = new EfDeletableEntityRepository<ArticleComment>(dbContext);
            this.categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstCookingHubUser = new CookingHubUser
            {
                Id = "1",
                FullName = "Stamat Stamatov",
                UserName = "Stamat99",
                Gender = Gender.Male,
            };

            this.firstCategory = new Category
            {
                Name = "Vegetables",
                Description = "Test category description",
            };

            this.firstArticle = new Article
            {
                Id = 1,
                Title = "Test article title",
                Description = "Test article description",
                ImagePath = "https://someimageurl.com",
                CategoryId = 1,
                UserId = "1",
            };

            this.firstArticleComment = new ArticleComment
            {
                ArticleId = this.firstArticle.Id,
                Content = "Nice article.",
                UserId = this.firstCookingHubUser.Id,
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedUsers();
            await this.SeedCategories();
            await this.SeedArticles();
        }

        private async Task SeedUsers()
        {
            await this.usersRepository.AddAsync(this.firstCookingHubUser);
            await this.usersRepository.SaveChangesAsync();
        }

        private async Task SeedCategories()
        {
            await this.categoriesRepository.AddAsync(this.firstCategory);
            await this.categoriesRepository.SaveChangesAsync();
        }

        private async Task SeedArticles()
        {
            await this.articlesRepository.AddAsync(this.firstArticle);
            await this.articlesRepository.SaveChangesAsync();
        }

        private async Task SeedArticleComments()
        {
            await this.articleCommentsRepository.AddAsync(this.firstArticleComment);
            await this.articleCommentsRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CookingHub.Models.ViewModels"));
    }
}
