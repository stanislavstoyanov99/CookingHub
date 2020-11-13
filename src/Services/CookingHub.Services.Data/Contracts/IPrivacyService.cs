namespace CookingHub.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.AdministratorInputModels.Privacy;
    using CookingHub.Models.ViewModels.Privacy;

    public interface IPrivacyService : IBaseDataService
    {
        Task<PrivacyDetailsViewModel> CreateAsync(PrivacyCreateInputModel privacyCreateInputModel);

        Task EditAsync(PrivacyEditViewModel privacyEditViewModel);

        Task<TViewModel> GetViewModelAsync<TViewModel>();
    }
}
