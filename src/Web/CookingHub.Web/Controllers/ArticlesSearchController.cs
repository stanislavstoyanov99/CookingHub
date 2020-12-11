namespace CookingHub.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Models.ViewModels;
    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesSearchController : ControllerBase
    {
        private const int ArticlesInSearchPage = 4;
        private readonly IArticlesService articlesService;

        public ArticlesSearchController(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }

        [HttpGet("{pageNumber}/{searchTitle}")]
        public async Task<ActionResult<PaginatedList<ArticleListingViewModel>>> Get(
            [FromRoute] int? pageNumber,
            [FromRoute] string searchTitle)
        {
            var articles = await Task.Run(() => this.articlesService
               .GetAllArticlesAsQueryeable<ArticleListingViewModel>()
               .Where(a => a.Title.ToLower().Contains(searchTitle.ToLower())));

            if (articles.Count() == 0)
            {
                return this.NotFound();
            }

            var articlesPaginated = await PaginatedList<ArticleListingViewModel>
                .CreateAsync(articles, pageNumber ?? 1, ArticlesInSearchPage);

            return articlesPaginated;
        }
    }
}
