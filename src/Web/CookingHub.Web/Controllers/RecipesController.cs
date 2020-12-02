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

        // Page number is used for pagination in UI in order to show only 12 recipes in All Tab
        // Category name is used in order to filter recipes by category (Categories are listed on the left side)
        public async Task<IActionResult> Index(int? pageNumber, string categoryName)
        {
            var allRecipes = await Task.Run(() =>
                this.recipesService.GetAllRecipesAsQueryeable<RecipeListingViewModel>());

            var recipesListingViewModel = await PaginatedList<RecipeListingViewModel>
                .CreateAsync(allRecipes, pageNumber ?? 1, PageSize);

            var categories = await this.categoriesService.GetAllCategoriesAsync<CategoryListingViewModel>();

            var model = new RecipePageViewModel
            {
                RecipesListingViewModel = recipesListingViewModel,
                Categories = categories,
                RecipesByCategory = new List<RecipeListingViewModel>(),
            };

            if (!string.IsNullOrEmpty(categoryName))
            {
                var recipesByCategory = await this.recipesService
                    .GetRecipesByCategoryAsync<RecipeListingViewModel>(categoryName);

                model.RecipesByCategory = recipesByCategory;
            }

            return this.View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var recipe = await this.recipesService.GetViewModelByIdAsync<RecipeDetailsViewModel>(id);

            return this.View(recipe);
        }
    }
}
