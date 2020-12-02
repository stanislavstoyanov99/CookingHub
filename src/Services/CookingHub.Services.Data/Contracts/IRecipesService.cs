namespace CookingHub.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.AdministratorInputModels.Recipes;
    using CookingHub.Models.ViewModels.Recipes;

    public interface IRecipesService : IBaseDataService
    {
        Task<RecipeDetailsViewModel> CreateAsync(RecipeCreateInputModel recipeCreateInputModel, string userId);

        Task EditAsync(RecipeEditViewModel recipeEditViewModel);

        Task<IEnumerable<TViewModel>> GetAllRecipesAsync<TViewModel>();

        IQueryable<TViewModel> GetAllRecipesAsQueryeable<TViewModel>();

        Task<IEnumerable<TViewModel>> GetRecipesByCategoryAsync<TViewModel>(string categoryName);
    }
}
