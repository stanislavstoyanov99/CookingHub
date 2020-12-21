namespace CookingHub.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.Recipes;
    using CookingHub.Models.ViewModels.Reviews;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ReviewsController : Controller
    {
        private readonly IReviewsService reviewsService;
        private readonly IRecipesService recipesService;
        private readonly UserManager<CookingHubUser> userManager;

        public ReviewsController(
            IReviewsService reviewsService,
            UserManager<CookingHubUser> userManager,
            IRecipesService recipesService)
        {
            this.reviewsService = reviewsService;
            this.recipesService = recipesService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateReviewInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var recipe = await this.recipesService
                    .GetViewModelByIdAsync<RecipeDetailsViewModel>(input.RecipeId);

                var model = new RecipeDetailsPageViewModel
                {
                    Recipe = recipe,
                    CreateReviewInputModel = input,
                };

                return this.View("/Views/Recipes/Details.cshtml", model);
            }

            var userId = this.userManager.GetUserId(this.User);
            input.UserId = userId;

            try
            {
                await this.reviewsService.CreateAsync(input);
            }
            catch (ArgumentException aex)
            {
                return this.BadRequest(aex.Message);
            }

            return this.RedirectToAction("Details", "Recipes", new { id = input.RecipeId });
        }
    }
}
