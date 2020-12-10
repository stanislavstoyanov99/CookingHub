namespace CookingHub.Web.Controllers
{
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
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
        private readonly IDeletableEntityRepository<CookingHubUser> userRepository;
        private readonly UserManager<CookingHubUser> userManager;

        public RecipesController(
            IRecipesService recipesService,
            ICategoriesService categoriesService,
            IReviewsService reviewsService,
            IDeletableEntityRepository<CookingHubUser> userRepository,
            UserManager<CookingHubUser> userManager)
        {
            this.recipesService = recipesService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
            this.reviewsService = reviewsService;
            this.userRepository = userRepository;
        }

        public async Task<IActionResult> Index(string categoryName, int? pageNumber)
        {
            this.TempData["CategoryName"] = categoryName;

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
            };

            return this.View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var recipe = await this.recipesService.GetViewModelByIdAsync<RecipeDetailsViewModel>(id);
            var reviews = await this.reviewsService.GetAll<ReviewDetailModel>(recipe.Id);

            recipe.Reviews = reviews;

            return this.View(recipe);
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

            await this.recipesService.CreateAsync(recipeCreateInputModel, user.Id);
            return this.RedirectToAction("Index", "Recipes");
        }
    }
}
