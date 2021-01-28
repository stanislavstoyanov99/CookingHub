namespace CookingHub.Web.Areas.Identity.Pages.Account
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [AllowAnonymous]
    public class Lockout : PageModel
    {
        public void OnGet()
        {
        }
    }
}
