namespace CookingHub.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public interface ICookingHubUsersService 
    {
        Task<IEnumerable<TViewModel>> GetAllCookingHubUsersAsync<TViewModel>();

        Task<TViewModel> GetViewModelByIdAsync<TViewModel>(string id);

        Task BanByIdAsync(string id);

        Task UnbanByIdAsync(string id);
    }
}
