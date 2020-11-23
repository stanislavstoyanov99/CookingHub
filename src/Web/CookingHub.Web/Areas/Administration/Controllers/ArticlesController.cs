namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.AdministratorInputModels.Articles;
    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : AdministrationController
    {
        private readonly IArticlesService articlesService;

        public ArticlesController(IArticlesService articlesService)
        {
            this.articlesService = articlesService;

        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateInputModel articleCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(articleCreateInputModel);
            }

            await this.articlesService.CreateAsync(articleCreateInputModel);
            return this.RedirectToAction("GetAll", "Articles", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var articleToEdit = await this.articlesService
                .GetViewModelByIdAsync<ArticlesEditViewModel>(id);

            return this.View(articleToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticlesEditViewModel articleEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(articleEditViewModel);
            }

            await this.articlesService.EditAsync(articleEditViewModel);
            return this.RedirectToAction("GetAll", "Articles", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var articlesToDelete = await this.articlesService.GetViewModelByIdAsync<ArticlesDetailsViewModel>(id);

            return this.View(articlesToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(ArticlesDetailsViewModel articlesDetailsViewModel)
        {
            await this.articlesService.DeleteByIdAsync(articlesDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "Articles", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var articles = await this.articlesService.GetAllArticlesAsync<ArticlesDetailsViewModel>();
            return this.View(articles);
        }
    }
}
}
