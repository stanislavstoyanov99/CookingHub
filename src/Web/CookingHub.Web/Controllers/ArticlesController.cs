namespace CookingHub.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Models.ViewModels;
    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Models.ViewModels.Categories;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : Controller
    {
        private const int PageSize = 9;
        private const int RecentArticlesCount = 6;
        private const int ArticlesByCategoryCount = 6;
        private readonly IArticlesService articlesService;
        private readonly ICategoriesService categoriesService;

        public ArticlesController(IArticlesService articlesService, ICategoriesService categoriesService)
        {
            this.articlesService = articlesService;
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var allArticles = await Task.Run(() =>
                this.articlesService.GetAllArticlesAsQueryeable<ArticleListingViewModel>());

            return this.View(await PaginatedList<ArticleListingViewModel>.CreateAsync(allArticles, pageNumber ?? 1, PageSize));
        }

        public async Task<IActionResult> Details(int id, string searchTitle)
        {
            var article = await this.articlesService
                .GetViewModelByIdAsync<ArticleListingViewModel>(id);

            if (!string.IsNullOrEmpty(searchTitle))
            {
                article = this.articlesService
                    .GetAllArticlesAsQueryeable<ArticleListingViewModel>()
                    .Where(a => a.Title.ToLower().Contains(searchTitle.ToLower()))
                    .FirstOrDefault();
            }

            var categories = await this.categoriesService
                .GetAllCategoriesAsync<CategoryListingViewModel>();

            var recentArticles = await this.articlesService
                .GetRecentArticlesAsync<RecentArticleListingViewModel>(RecentArticlesCount);

            var viewModel = new DetailsListingViewModel
            {
                ArticleListingViewModel = article,
                Categories = categories,
                RecentArticles = recentArticles,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> ByName(int? pageNumber, string categoryName)
        {
            var articlesByCategoryName = await Task.Run(() => this.articlesService
                .GetAllArticlesByCategoryNameAsQueryeable<ArticleListingViewModel>(categoryName));

            if (articlesByCategoryName.Count() == 0)
            {
                return this.NotFound();
            }

            this.TempData["CategoryName"] = categoryName;
            var articlesByCategoryNamePaginated = await PaginatedList<ArticleListingViewModel>
                    .CreateAsync(articlesByCategoryName, pageNumber ?? 1, ArticlesByCategoryCount);

            return this.View(articlesByCategoryNamePaginated);
        }
    }
}
