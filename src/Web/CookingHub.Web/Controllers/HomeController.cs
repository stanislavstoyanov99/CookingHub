namespace CookingHub.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using CookingHub.Common;
    using CookingHub.Common.Attributes;
    using CookingHub.Models.ViewModels;
    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Models.ViewModels.Home;
    using CookingHub.Models.ViewModels.Privacy;
    using CookingHub.Models.ViewModels.Recipes;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private const int TopRecipesCounter = 6;
        private const int RecentArticlesCounter = 2;
        private readonly IPrivacyService privacyService;
        private readonly IArticlesService articlesService;
        private readonly IRecipesService recipesService;

        public HomeController(
            IPrivacyService privacyService,
            IArticlesService articlesService,
            IRecipesService recipesService)
        {
            this.privacyService = privacyService;
            this.articlesService = articlesService;
            this.recipesService = recipesService;
        }

        public async Task<IActionResult> Index()
        {
            var topRecipes = await this
                .recipesService.GetTopRecipesAsync<RecipeListingViewModel>(TopRecipesCounter);

            var recentArticles = await this
                .articlesService.GetRecentArticlesAsync<ArticleListingViewModel>(RecentArticlesCounter);

            var gallery = await this
                .recipesService.GetAllRecipesAsync<GalleryViewModel>();

            var model = new HomePageViewModel
            {
                TopRecipes = topRecipes,
                RecentArticles = recentArticles,
                Gallery = gallery,
            };

            return this.View(model);
        }

        [NoDirectAccessAttribute]
        public IActionResult ThankYouSubscription(string email)
        {
            return this.View("SuccessfullySubscribed", email);
        }

        [NoDirectAccessAttribute]
        public IActionResult RequestNewPassword(ChangePasswordViewModel model)
        {
            return this.View(model);
        }

        public async Task<IActionResult> Privacy()
        {
            var privacy = await this.privacyService.GetViewModelAsync<PrivacyDetailsViewModel>();

            return this.View(privacy);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        public IActionResult HttpError(HttpErrorViewModel errorViewModel)
        {
            if (errorViewModel.StatusCode == 404)
            {
                return this.View(errorViewModel);
            }

            return this.Error();
        }
    }
}
