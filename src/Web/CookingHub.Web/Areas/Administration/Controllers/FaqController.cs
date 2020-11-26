namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.AdministratorInputModels.Faq;
    using CookingHub.Models.ViewModels.Faq;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class FaqController : AdministrationController
    {
        private readonly IFaqService faqService;

        public FaqController(IFaqService faqService)
        {
            this.faqService = faqService;
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
        public async Task<IActionResult> Create(FaqCreateInputModel faqCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(faqCreateInputModel);
            }

            await this.faqService.CreateAsync(faqCreateInputModel);
            return this.RedirectToAction("GetAll", "About", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var faqToEdit = await this.faqService
                .GetViewModelByIdAsync<FaqEditViewModel>(id);

            return this.View(faqToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FaqEditViewModel faqEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(faqEditViewModel);
            }

            await this.faqService.EditAsync(faqEditViewModel);
            return this.RedirectToAction("GetAll", "About", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var faqToDelete = await this.faqService.GetViewModelByIdAsync<FaqDetailsViewModel>(id);
            return this.View(faqToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(FaqDetailsViewModel faqDetailsViewModel)
        {
            await this.faqService.DeleteByIdAsync(faqDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "About", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var faqs = await this.faqService.GetAllFaqsAsync<FaqDetailsViewModel>();
            return this.View(faqs);
        }
    }
}
