namespace CookingHub.Web.Controllers
{
    using System.Threading.Tasks;

    using CookingHub.Models.ViewModels;
    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : Controller
    {
        private const int PageSize = 9;
        private readonly IArticlesService articlesService;

        public ArticlesController(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var allArticles = await Task.Run(() =>
                this.articlesService.GetAllArticlesAsQueryeable<ArticleListingViewModel>());

            return this.View(await PaginatedList<ArticleListingViewModel>.CreateAsync(allArticles, pageNumber ?? 1, PageSize));
        }

        public IActionResult Details(int id)
        {
            return this.View();
        }
    }
}
