namespace CookingHub.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CookingHub.Data;
    using CookingHub.Data.Models;
    using CookingHub.Data.Repositories;
    using CookingHub.Models.InputModels.AdministratorInputModels.Faq;
    using CookingHub.Models.ViewModels.Faq;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class FaqServiceTests : IAsyncDisposable
    {
        private readonly IFaqService faqService;
        private EfDeletableEntityRepository<FaqEntry> faqEntriesRepository;
        private SqliteConnection connection;
        private FaqEntry firstFaqEntry;

        public FaqServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.faqService = new FaqService(this.faqEntriesRepository);
        }

        [Fact]
        public async Task TestAddingFaqEntry()
        {
            var model = new FaqCreateInputModel
            {
                Question = "How can I register?",
                Answer = "Use the register form.",
            };

            await this.faqService.CreateAsync(model);
            var count = await this.faqEntriesRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfFaqEntryProperties()
        {
            var model = new FaqCreateInputModel
            {
                Question = "What do you think about the website?",
                Answer = "I think it is a nice place to visit.",
            };

            await this.faqService.CreateAsync(model);

            var faqEntry = await this.faqEntriesRepository.All().FirstOrDefaultAsync();

            Assert.Equal(model.Question, faqEntry.Question);
            Assert.Equal(model.Answer, faqEntry.Answer);
        }

        [Fact]
        public async Task CheckIfAddingFaqEntryThrowsArgumentException()
        {
            this.SeedDatabase();

            var faqEntry = new FaqCreateInputModel
            {
                Question = this.firstFaqEntry.Question,
                Answer = this.firstFaqEntry.Answer,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.faqService.CreateAsync(faqEntry));

            Assert.Equal(string.Format(ExceptionMessages.FaqAlreadyExists, faqEntry.Question, faqEntry.Answer), exception.Message);
        }

        [Fact]
        public async Task CheckIfAddingFaqEntryReturnsViewModel()
        {
            var faqEntry = new FaqCreateInputModel
            {
                Question = "What do you think about our reviews?",
                Answer = "There are so beautiful.",
            };

            var viewModel = await this.faqService.CreateAsync(faqEntry);
            var dbEntry = await this.faqEntriesRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel.Id);
            Assert.Equal(dbEntry.Question, viewModel.Question);
            Assert.Equal(dbEntry.Answer, viewModel.Answer);
        }

        [Fact]
        public async Task CheckIfDeletingFaqEntryWorksCorrectly()
        {
            this.SeedDatabase();

            await this.faqService.DeleteByIdAsync(this.firstFaqEntry.Id);

            var count = await this.faqEntriesRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingFaqEntryReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.faqService.DeleteByIdAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.FaqNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfEditingFaqEntryWorksCorrectly()
        {
            this.SeedDatabase();

            var faqEditViewModel = new FaqEditViewModel
            {
                Id = this.firstFaqEntry.Id,
                Answer = "Changed Answer",
                Question = "Changed Question",
            };

            await this.faqService.EditAsync(faqEditViewModel);

            Assert.Equal(faqEditViewModel.Answer, this.firstFaqEntry.Answer);
            Assert.Equal(faqEditViewModel.Question, this.firstFaqEntry.Question);
        }

        [Fact]
        public async Task CheckIfEditingFaqEntryReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var faqEditViewModel = new FaqEditViewModel
            {
                Id = 3,
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.faqService.EditAsync(faqEditViewModel));
            Assert.Equal(string.Format(ExceptionMessages.FaqNotFound, faqEditViewModel.Id), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllFaqsAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.faqService.GetAllFaqsAsync<FaqDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetAllFaqsAsyncWorksCorrectlyWithZeroFaqs()
        {
            var result = await this.faqService.GetAllFaqsAsync<FaqDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfGetFaqViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new FaqDetailsViewModel
            {
                Id = this.firstFaqEntry.Id,
                Answer = this.firstFaqEntry.Answer,
                Question = this.firstFaqEntry.Question,
            };

            var viewModel = await this.faqService.GetViewModelByIdAsync<FaqDetailsViewModel>(this.firstFaqEntry.Id);

            var expectedObj = JsonConvert.SerializeObject(expectedModel);
            var actualResultObj = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(expectedObj, actualResultObj);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.faqService.GetViewModelByIdAsync<FaqDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.FaqNotFound, 3), exception.Message);
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

            this.faqEntriesRepository = new EfDeletableEntityRepository<FaqEntry>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstFaqEntry = new FaqEntry
            {
                Question = "Test Question?",
                Answer = "Test Answer.",
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedFaqEntries();
        }

        private async Task SeedFaqEntries()
        {
            await this.faqEntriesRepository.AddAsync(this.firstFaqEntry);

            await this.faqEntriesRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CookingHub.Models.ViewModels"));
    }
}
