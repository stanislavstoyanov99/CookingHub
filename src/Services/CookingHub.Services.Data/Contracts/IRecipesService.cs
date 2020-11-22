namespace CookingHub.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.AdministratorInputModels.Recipes;
    using CookingHub.Models.ViewModels.Recipes;

    public interface IRecipesService : IBaseDataService
    {
        Task<RecipesDetailsViewModel> CreateAsync(RecipesCreateInputModel recipesCreateInputModel);

        Task EditAsync(RecipesEditViewModel recipesEditViewModel);

        Task<IEnumerable<TEntity>> GetAllRecipesAsync<TEntity>();
    }
}
