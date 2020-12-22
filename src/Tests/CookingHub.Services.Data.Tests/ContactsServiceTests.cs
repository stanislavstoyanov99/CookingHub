namespace CookingHub.Services.Data.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using CookingHub.Data;
    using CookingHub.Data.Models;
    using CookingHub.Data.Repositories;
    using CookingHub.Models.ViewModels.Contacts;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;
    using CookingHub.Services.Messaging;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class ContactsServiceTests : IAsyncDisposable, IClassFixture<Configuration>
    {
        private readonly IEmailSender emailSender;
        private readonly IContactsService contactsService;

        private EfRepository<ContactFormEntry> userContactsRepository;
        private SqliteConnection connection;

        private ContactFormEntry firstUserContactFormEntry;

        public ContactsServiceTests(Configuration configuration)
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.emailSender = new SendGridEmailSender(configuration.ConfigurationRoot["SendGridCookingHub:ApiKey"]);
            this.contactsService = new ContactsService(this.userContactsRepository, this.emailSender);
        }

        [Fact]
        public async Task CheckIfSendContactToAdminAsyncWorksCorrectly()
        {
            var model = new ContactFormEntryViewModel
            {
                FirstName = "Pesho",
                LastName = "Petrov",
                Email = "pesho777@gmail.com",
                Subject = "Question about recipes",
                Content = "Sample content about recipes",
            };

            await this.contactsService.SendContactToAdminAsync(model);
            var count = await this.userContactsRepository.All().CountAsync();

            Assert.Equal(1, count);
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

            this.userContactsRepository = new EfRepository<ContactFormEntry>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstUserContactFormEntry = new ContactFormEntry
            {
                FirstName = "Stanislav",
                LastName = "Stoyanov",
                Email = "stan_99@gmail.com",
                Subject = "Question about register form",
                Content = "I have to ask you something connected with the register form.",
            };
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CookingHub.Models.ViewModels"));
    }
}
