namespace CookingHub.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Models.ViewModels.Categories;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ArticleCommentsController : Controller
    {
        private readonly IArticleCommentsService articleCommentsService;
        private readonly IArticlesService articlesService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<CookingHubUser> userManager;

        public ArticleCommentsController(
            IArticleCommentsService articleCommentsService,
            UserManager<CookingHubUser> userManager,
            IArticlesService articlesService,
            ICategoriesService categoriesService)
        {
            this.articleCommentsService = articleCommentsService;
            this.userManager = userManager;
            this.articlesService = articlesService;
            this.categoriesService = categoriesService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(DetailsListingViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                var article = await this.articlesService
                    .GetViewModelByIdAsync<ArticleListingViewModel>(viewModel.CreateArticleCommentInputModel.ArticleId);

                var categories = await this.categoriesService
                    .GetAllCategoriesAsync<CategoryListingViewModel>();

                var recentArticles = await this.articlesService
                    .GetRecentArticlesAsync<RecentArticleListingViewModel>(6);

                var model = new DetailsListingViewModel
                {
                    ArticleListingViewModel = article,
                    Categories = categories,
                    RecentArticles = recentArticles,
                    CreateArticleCommentInputModel = viewModel.CreateArticleCommentInputModel,
                };

                return this.View("/Views/Articles/Details.cshtml", model);
            }

            var parentId = viewModel.CreateArticleCommentInputModel.ParentId == 0 ?
                    (int?)null : viewModel.CreateArticleCommentInputModel.ParentId;

            if (parentId.HasValue)
            {
                if (!await this.articleCommentsService.IsInArticleId(parentId.Value, viewModel.CreateArticleCommentInputModel.ArticleId))
                {
                    return this.BadRequest();
                }
            }

            var userId = this.userManager.GetUserId(this.User);

            try
            {
                await this.articleCommentsService.CreateAsync(
                    viewModel.CreateArticleCommentInputModel.ArticleId,
                    userId,
                    viewModel.CreateArticleCommentInputModel.Content.Trim(),
                    parentId);
            }
            catch (ArgumentException aex)
            {
                return this.BadRequest(aex.Message);
            }

            return this.RedirectToAction("Details", "Articles", new { id = viewModel.CreateArticleCommentInputModel.ArticleId });
        }
    }
}
