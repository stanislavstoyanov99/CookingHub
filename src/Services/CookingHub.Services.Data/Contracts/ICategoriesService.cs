namespace CookingHub.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.AdministratorInputModels.Categories;
    using CookingHub.Models.ViewModels.Categories;

    public interface ICategoriesService : IBaseDataService
    {
        Task<CategoryDetailsViewModel> CreateAsync(CategoryCreateInputModel categoryCreateInputModel);

        Task EditAsync(CategoryEditViewModel categoryEditViewModel);

        Task<IEnumerable<TEntity>> GetAllCategoriesAsync<TEntity>();

        Task<TViewModel> GetCategoryAsync<TViewModel>(string name);
    }
}
