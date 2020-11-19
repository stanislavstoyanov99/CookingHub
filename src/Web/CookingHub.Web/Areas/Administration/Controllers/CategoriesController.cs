namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.AdministratorInputModels.Categories;
    using CookingHub.Models.ViewModels.Categories;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
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
        public async Task<IActionResult> Create(CategoryCreateInputModel categoryCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(categoryCreateInputModel);
            }

            await this.categoriesService.CreateAsync(categoryCreateInputModel);
            return this.RedirectToAction("GetAll", "Categories", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categoryToEdit = await this.categoriesService
                .GetViewModelByIdAsync<CategoryEditViewModel>(id);

            return this.View(categoryToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditViewModel categoryEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(categoryEditViewModel);
            }

            await this.categoriesService.EditAsync(categoryEditViewModel);
            return this.RedirectToAction("GetAll", "Categories", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var categoryToDelete = await this.categoriesService.GetViewModelByIdAsync<CategoryDetailsViewModel>(id);
            return this.View(categoryToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(CategoryDetailsViewModel categoryViewModel)
        {
            await this.categoriesService.DeleteByIdAsync(categoryViewModel.Id);
            return this.RedirectToAction("GetAll", "Categories", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var categories = await this.categoriesService.GetAllCategoriesAsync<CategoryDetailsViewModel>();
            return this.View(categories);
        }
    }
}
