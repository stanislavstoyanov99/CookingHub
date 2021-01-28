namespace CookingHub.Web.Controllers
{
    using System.Threading.Tasks;

    using CookingHub.Common.Attributes;
    using CookingHub.Data.Models;
    using CookingHub.Models.InputModels.AdministratorInputModels.Recipes;
    using CookingHub.Models.ViewModels;
    using CookingHub.Models.ViewModels.Categories;
    using CookingHub.Models.ViewModels.Recipes;
    using CookingHub.Models.ViewModels.Reviews;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class RecipesController : Controller
    {
        private const int PageSize = 12;
        private readonly IRecipesService recipesService;
        private readonly IReviewsService reviewsService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<CookingHubUser> userManager;

        public RecipesController(
            IRecipesService recipesService,
            ICategoriesService categoriesService,
            IReviewsService reviewsService,
            UserManager<CookingHubUser> userManager)
        {
            this.recipesService = recipesService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
            this.reviewsService = reviewsService;
        }

        [ServiceFilter(typeof(PasswordExpirationCheckAttribute))]
        public async Task<IActionResult> Index(string categoryName, int? pageNumber)
        {
            this.TempData["CategoryName"] = categoryName;

            var recipes = this.recipesService
                .GetAllRecipesByFilterAsQueryeable<RecipeListingViewModel>(categoryName);

            var recipesPaginated = await PaginatedList<RecipeListingViewModel>
                .CreateAsync(recipes, pageNumber ?? 1, PageSize);

            var categories = await this.categoriesService
                .GetAllCategoriesAsync<CategoryListingViewModel>();

            var reviews = await this.reviewsService
                .GetTopReviews<ReviewListingViewModel>();

            var model = new RecipeIndexPageViewModel
            {
                RecipesPaginated = recipesPaginated,
                Categories = categories,
                Reviews = reviews,
            };

            return this.View(model);
        }

        [ServiceFilter(typeof(PasswordExpirationCheckAttribute))]
        public async Task<IActionResult> Details(int id)
        {
            var recipe = await this.recipesService.GetViewModelByIdAsync<RecipeDetailsViewModel>(id);

            var model = new RecipeDetailsPageViewModel
            {
                Recipe = recipe,
                CreateReviewInputModel = new CreateReviewInputModel(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> RecipeList()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var recipes = await this.recipesService.GetAllRecipesByUserId<RecipeDetailsViewModel>(user.Id);
            var categories = await this.categoriesService.GetAllCategoriesAsync<CategoryDetailsViewModel>();

            var model = new MyRecipeDetailsViewModel
            {
                Recipes = recipes,
                RecipeEditViewModel = new RecipeEditViewModel { Categories = categories, },
            };

            return this.View(model);
        }

        public async Task<IActionResult> Submit()
        {
            var categories = await this.categoriesService
                .GetAllCategoriesAsync<CategoryDetailsViewModel>();

            var model = new RecipeCreateInputModel
            {
                Categories = categories,
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Submit(RecipeCreateInputModel recipeCreateInputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                var categories = await this.categoriesService
                  .GetAllCategoriesAsync<CategoryDetailsViewModel>();

                recipeCreateInputModel.Categories = categories;

                return this.View(recipeCreateInputModel);
            }

            var recipe = await this.recipesService.CreateAsync(recipeCreateInputModel, user.Id);
            return this.RedirectToAction("Details", "Recipes", new { id = recipe.Id });
        }

        [Authorize]
        public async Task<IActionResult> Remove(int id)
        {
            await this.recipesService.DeleteByIdAsync(id);

            return this.RedirectToAction("RecipeList", "Recipes");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(MyRecipeDetailsViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model.RecipeEditViewModel);
            }

            await this.recipesService.EditAsync(model.RecipeEditViewModel);
            return this.RedirectToAction("RecipeList", "Recipes");
        }

        [HttpGet]
        public async Task<RecipeDetailsViewModel> GetRecipe(int id)
        {
            var recipe = await this.recipesService.GetViewModelByIdAsync<RecipeDetailsViewModel>(id);
            return recipe;
        }
    }
}
