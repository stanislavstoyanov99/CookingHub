namespace CookingHub.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using CookingHub.Models.ViewModels.Contacts;

    public interface IContactsService
    {
        Task SendContactToAdminAsync(ContactFormEntryViewModel contactFormEntryViewModel);
    }
}
