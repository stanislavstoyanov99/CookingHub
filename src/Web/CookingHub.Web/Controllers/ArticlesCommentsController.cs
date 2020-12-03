namespace CookingHub.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using CookingHub.Data.Models;
    using CookingHub.Models.InputModels.ArticlesComments;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesCommentsController : Controller
    {
        private readonly IArticlesCommentsService articlescommentsService;
        private readonly UserManager<CookingHubUser> userManager;

        public ArticlesCommentsController(
            IArticlesCommentsService commentsService,
            UserManager<CookingHubUser> userManager)
        {
            this.articlescommentsService = commentsService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateArticleCommentInputModel input)
        {
            var parentId = input.ParentId == 0 ? (int?)null : input.ParentId;

            if (parentId.HasValue)
            {
                if (!await this.articlescommentsService.IsInArticlesId(parentId.Value, input.ArticleId))
                {
                    return this.BadRequest();
                }
            }

            var userId = this.userManager.GetUserId(this.User);

            try
            {
                await this.articlescommentsService.CreateAsync(input.ArticleId, userId, input.Content, parentId);
            }
            catch (ArgumentException aex)
            {
                return this.BadRequest(aex.Message);
            }

            return this.RedirectToAction("Details", "Articles", new { id = input.ArticleId });
        }
    }
}
