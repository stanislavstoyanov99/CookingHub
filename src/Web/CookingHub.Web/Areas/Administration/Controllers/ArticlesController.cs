namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookingHub.Data.Models;
    using CookingHub.Models.InputModels.AdministratorInputModels.Articles;
    using CookingHub.Models.ViewModels;
    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Models.ViewModels.Categories;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : AdministrationController
    {
        private const int PageSize = 6;
        private readonly IArticlesService articlesService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<CookingHubUser> userManager;

        public ArticlesController(
            IArticlesService articlesService,
            ICategoriesService categoriesService,
            UserManager<CookingHubUser> userManager)
        {
            this.articlesService = articlesService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Create()
        {
            var categories = await this.categoriesService
                   .GetAllCategoriesAsync<CategoryDetailsViewModel>();

            var model = new ArticleCreateInputModel
            {
                Categories = categories,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateInputModel articleCreateInputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                var categories = await this.categoriesService
                   .GetAllCategoriesAsync<CategoryDetailsViewModel>();

                articleCreateInputModel.Categories = categories;

                return this.View(articleCreateInputModel);
            }

            await this.articlesService.CreateAsync(articleCreateInputModel, user.Id);
            return this.RedirectToAction("GetAll", "Articles", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categories = await this.categoriesService
                  .GetAllCategoriesAsync<CategoryDetailsViewModel>();

            var articleToEdit = await this.articlesService
                .GetViewModelByIdAsync<ArticleEditViewModel>(id);

            articleToEdit.Categories = categories;

            return this.View(articleToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticleEditViewModel articleEditViewModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                var categories = await this.categoriesService
                   .GetAllCategoriesAsync<CategoryDetailsViewModel>();

                articleEditViewModel.Categories = categories;

                return this.View(articleEditViewModel);
            }

            await this.articlesService.EditAsync(articleEditViewModel, user.Id);
            return this.RedirectToAction("GetAll", "Articles", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var articleToDelete = await this.articlesService.GetViewModelByIdAsync<ArticleDetailsViewModel>(id);

            return this.View(articleToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(ArticleDetailsViewModel articlesDetailsViewModel)
        {
            await this.articlesService.DeleteByIdAsync(articlesDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "Articles", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll(int? pageNumber)
        {
            var articles = this.articlesService
                .GetAllArticlesAsQueryeable<ArticleDetailsViewModel>();

            var articlesPaginated = await PaginatedList<ArticleDetailsViewModel>
                .CreateAsync(articles, pageNumber ?? 1, PageSize);

            return this.View(articlesPaginated);
        }
    }
}
