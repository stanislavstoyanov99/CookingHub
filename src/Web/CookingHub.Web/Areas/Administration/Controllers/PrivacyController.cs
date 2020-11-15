namespace CookingHub.Web.Areas.Administration.Controllers
{
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
    }
}
