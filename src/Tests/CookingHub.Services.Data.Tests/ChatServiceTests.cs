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
    using CookingHub.Models.ViewModels.Chat;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class ChatServiceTests : IAsyncDisposable
    {
        private readonly IChatService chatService;
        private EfDeletableEntityRepository<Message> messagesRepository;
        private EfDeletableEntityRepository<CookingHubUser> usersRepository;
        private SqliteConnection connection;

        private Message firstMessage;
        private CookingHubUser cookingHubUser;

        public ChatServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.chatService = new ChatService(this.messagesRepository);
        }

        [Fact]
        public async Task TestAddingMessage()
        {
            await this.SeedUsers();

            var model = new MessageInputModel
            {
                Content = "Hello, Peter",
                UserName = this.cookingHubUser.UserName,
                UserId = this.cookingHubUser.Id,
            };

            await this.chatService.CreateAsync(model);
            var count = await this.messagesRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfMessageProperties()
        {
            await this.SeedUsers();

            var model = new MessageInputModel
            {
                Content = "How are you",
                UserName = this.cookingHubUser.UserName,
                UserId = this.cookingHubUser.Id,
            };

            await this.chatService.CreateAsync(model);

            var message = await this.messagesRepository.All().FirstOrDefaultAsync();

            Assert.Equal("How are you", message.Content);
            Assert.Equal(model.UserName, message.UserName);
            Assert.Equal(model.UserId, message.UserId);
        }

        [Fact]
        public async Task CheckIfAddingMessageReturnsViewModel()
        {
            await this.SeedUsers();

            var message = new MessageInputModel
            {
                Content = "Filters",
                UserName = this.cookingHubUser.UserName,
                UserId = this.cookingHubUser.Id,
            };

            var viewModel = await this.chatService.CreateAsync(message);
            var dbEntry = await this.messagesRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel.Id);
            Assert.Equal(dbEntry.Content, viewModel.Content);
            Assert.Equal(dbEntry.UserName, viewModel.UserUsername);
        }

        [Fact]
        public async Task CheckIfDeletingMessageWorksCorrectly()
        {
            this.SeedDatabase();

            await this.chatService.DeleteByIdAsync(this.firstMessage.Id);

            var count = await this.messagesRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingMessageReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.chatService.DeleteByIdAsync(3));

            Assert.Equal(string.Format(ExceptionMessages.MessageNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllMessagesAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.chatService.GetAllMessagesAsync<MessageViewModel>();

            var count = result.Count();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetMessageViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new MessageViewModel
            {
                Id = this.firstMessage.Id,
                Content = this.firstMessage.Content,
                UserUsername = this.firstMessage.UserName,
                CreatedOn = this.firstMessage.CreatedOn,
            };
            expectedModel.CreatedOn = DateTime.Parse(expectedModel.CreatedOn.ToShortDateString());

            var viewModel = await this.chatService
                .GetViewModelByIdAsync<MessageViewModel>(this.firstMessage.Id);
            viewModel.CreatedOn = DateTime.Parse(viewModel.CreatedOn.ToShortDateString());

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
                async () => await this.chatService.GetViewModelByIdAsync<MessageViewModel>(3));

            Assert.Equal(string.Format(ExceptionMessages.MessageNotFound, 3), exception.Message);
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
            this.messagesRepository = new EfDeletableEntityRepository<Message>(dbContext);
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

            this.firstMessage = new Message
            {
                Content = "Hello, World",
                UserId = this.cookingHubUser.Id,
                UserName = this.cookingHubUser.UserName,
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedUsers();
            await this.SeedMessages();
        }

        private async Task SeedMessages()
        {
            await this.messagesRepository.AddAsync(this.firstMessage);

            await this.messagesRepository.SaveChangesAsync();
        }

        private async Task SeedUsers()
        {
            await this.usersRepository.AddAsync(this.cookingHubUser);

            await this.usersRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CookingHub.Models.ViewModels"));
    }
}
