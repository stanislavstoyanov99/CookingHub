namespace CookingHub.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookingHub.Models.ViewModels.Chat;

    public interface IChatService : IBaseDataService
    {
        Task<MessageViewModel> CreateAsync(MessageInputModel messageCreateInputModel);

        Task<IEnumerable<TViewModel>> GetAllMessagesAsync<TViewModel>();
    }
}
