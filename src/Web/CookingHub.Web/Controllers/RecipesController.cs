namespace CookingHub.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookingHub.Models.ViewModels;
    using CookingHub.Models.ViewModels.Categories;
    using CookingHub.Models.ViewModels.Recipes;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class RecipesController : Controller
    {
        private const int PageSize = 12;
        private readonly IRecipesService recipesService;
        private readonly ICategoriesService categoriesService;

        public RecipesController(IRecipesService recipesService, ICategoriesService categoriesService)
        {
            this.recipesService = recipesService;
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index(string categoryName, int? pageNumber)
        {
            var recipes = this.recipesService
                .GetAllRecipesByFilterAsQueryeable<RecipeListingViewModel>(categoryName);

            var recipesPaginated = await PaginatedList<RecipeListingViewModel>
                .CreateAsync(recipes, pageNumber ?? 1, PageSize);

            var categories = await this.categoriesService
                .GetAllCategoriesAsync<CategoryListingViewModel>();

            var model = new RecipePageViewModel
            {
                RecipesPaginated = recipesPaginated,
                Categories = categories,
                SelectedTab = categoryName,
            };

            return this.View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var recipe = await this.recipesService.GetViewModelByIdAsync<RecipeDetailsViewModel>(id);

            return this.View(recipe);
        }
    }
}
