namespace CookingHub.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICookingHubUsersService
    {
        Task BanByIdAsync(string id);

        Task UnbanByIdAsync(string id);

        Task<IEnumerable<TViewModel>> GetAllCookingHubUsersAsync<TViewModel>();

        Task<TViewModel> GetViewModelByIdAsync<TViewModel>(string id);
    }
}
