namespace CookingHub.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using CookingHub.Models.ViewModels;
    using CookingHub.Models.ViewModels.Privacy;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IPrivacyService privacyService;

        public HomeController(IPrivacyService privacyService)
        {
            this.privacyService = privacyService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Privacy()
        {
            var privacy = await this.privacyService.GetViewModelAsync<PrivacyDetailsViewModel>();

            return this.View(privacy);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
