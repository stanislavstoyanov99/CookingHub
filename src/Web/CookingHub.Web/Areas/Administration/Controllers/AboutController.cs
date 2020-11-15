namespace CookingHub.Web.Areas.Administration.Controllers
{
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class AboutController : AdministrationController
    {
        private readonly IAboutService aboutService;

        public AboutController(IAboutService aboutService)
        {
            this.aboutService = aboutService;
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}
