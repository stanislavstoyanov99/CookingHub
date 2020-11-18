namespace CookingHub.Web.Controllers
{
    using System.Threading.Tasks;

    using CookingHub.Models.ViewModels.About;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class AboutController : Controller
    {
        private readonly IAboutService aboutService;

        public AboutController(IAboutService aboutService)
        {
            this.aboutService = aboutService;
        }

        public async Task<IActionResult> Index()
        {
            var faqs = await this.aboutService.GetAllFaqsAsync<FaqDetailsViewModel>();

            return this.View(faqs);
        }
    }
}
