namespace CookingHub.Services.Data
{
    using System.Threading.Tasks;

    using CookingHub.Common;
    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.Contacts;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Messaging;

    public class ContactsService : IContactsService
    {
        private readonly IRepository<ContactFormEntry> userContactsRepository;
        private readonly IEmailSender emailSender;

        public ContactsService(
            IRepository<ContactFormEntry> userContactsRepository,
            IEmailSender emailSender)
        {
            this.userContactsRepository = userContactsRepository;
            this.emailSender = emailSender;
        }

        public async Task SendContactToAdminAsync(ContactFormEntryViewModel contactFormEntryViewModel)
        {
            var contactFormEntry = new ContactFormEntry
            {
                FirstName = contactFormEntryViewModel.FirstName,
                LastName = contactFormEntryViewModel.LastName,
                Email = contactFormEntryViewModel.Email,
                Subject = contactFormEntryViewModel.Subject,
                Content = contactFormEntryViewModel.Content,
            };

            await this.userContactsRepository.AddAsync(contactFormEntry);
            await this.userContactsRepository.SaveChangesAsync();

            await this.emailSender.SendEmailAsync(
                contactFormEntryViewModel.Email,
                string.Concat(contactFormEntryViewModel.FirstName, " ", contactFormEntryViewModel.LastName),
                GlobalConstants.SystemEmail,
                contactFormEntryViewModel.Subject,
                contactFormEntryViewModel.Content);
        }
    }
}
