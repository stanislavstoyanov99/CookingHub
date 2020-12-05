namespace CookingHub.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Models.ViewModels;
    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Models.ViewModels.Privacy;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IPrivacyService privacyService;
        private readonly IArticlesService articlesService;

        public HomeController(IPrivacyService privacyService , IArticlesService articlesService)
        {
            this.privacyService = privacyService;
            this.articlesService = articlesService;
        }

        public async Task<IActionResult> Index()
        {
            var allArticles = await Task.Run(() =>
                 this.articlesService.GetAllArticlesAsQueryeable<ArticleListingViewModel>());
            var top2 = allArticles.OrderByDescending(x=>x.CreatedOn).Take(2).ToList();
            return this.View(top2);
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
    }
}
