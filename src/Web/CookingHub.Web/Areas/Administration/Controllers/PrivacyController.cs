namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.AdministratorInputModels.Privacy;
    using CookingHub.Models.ViewModels.Privacy;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class PrivacyController : AdministrationController
    {
        private readonly IPrivacyService privacyService;

        public PrivacyController(IPrivacyService privacyService)
        {
            this.privacyService = privacyService;
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
        public async Task<IActionResult> Create(PrivacyCreateInputModel privacyCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(privacyCreateInputModel);
            }

            await this.privacyService.CreateAsync(privacyCreateInputModel);
            return this.RedirectToAction("Privacy", "Home", new { area = string.Empty });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var privacyToEdit = await this.privacyService
                .GetViewModelByIdAsync<PrivacyEditViewModel>(id);

            return this.View(privacyToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PrivacyEditViewModel privacyEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(privacyEditViewModel);
            }

            await this.privacyService.EditAsync(privacyEditViewModel);
            return this.RedirectToAction("Privacy", "Home", new { area = string.Empty });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var privacyToDelete = await this.privacyService.GetViewModelByIdAsync<PrivacyDetailsViewModel>(id);

            return this.View(privacyToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(PrivacyDetailsViewModel privacyDetailsViewModel)
        {
            await this.privacyService.DeleteByIdAsync(privacyDetailsViewModel.Id);
            return this.RedirectToAction("Privacy", "Home", new { area = string.Empty });
        }
    }
}
