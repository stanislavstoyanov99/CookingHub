namespace CookingHub.Web.Controllers
{
    using CookingHub.Data.Models;
    using CookingHub.Models.InputModels.Reviews;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReviewsController : Controller
    {
        private readonly IReviewsService reviewsService;
        private readonly UserManager<CookingHubUser> userManager;

        public ReviewsController(
            IReviewsService reviewsService,
            UserManager<CookingHubUser> userManager)
        {
            this.reviewsService = reviewsService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateReviewInputModel input)
        {
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
