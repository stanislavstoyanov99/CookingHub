namespace CookingHub.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Web.Areas.Identity.Pages.Account.Manage.InputModels;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class ChangePassword : PageModel
    {
        private readonly UserManager<CookingHubUser> userManager;
        private readonly SignInManager<CookingHubUser> signInManager;
        private readonly ILogger<ChangePassword> logger;
        private IDeletableEntityRepository<CookingHubUser> usersRepository;

        public ChangePassword(
            UserManager<CookingHubUser> userManager,
            SignInManager<CookingHubUser> signInManager,
            ILogger<ChangePassword> logger,
            IDeletableEntityRepository<CookingHubUser> usersRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.usersRepository = usersRepository;
        }

        [BindProperty]
        public ChangePasswordInputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string message)
        {
            if (message != null)
            {
                this.ViewData["InfoMessage"] = message;
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var hasPassword = await this.userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return this.RedirectToPage("./SetPassword");
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var changePasswordResult = await this.userManager
                .ChangePasswordAsync(user, this.Input.OldPassword, this.Input.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.Page();
            }

            var dbUser = this.usersRepository.All().First(x => x.UserName == this.User.Identity.Name);
            dbUser.ChangedPasswordOn = DateTime.UtcNow;
            this.usersRepository.Update(dbUser);
            await this.usersRepository.SaveChangesAsync();

            await this.signInManager.RefreshSignInAsync(user);
            this.logger.LogInformation("User changed their password successfully.");
            this.StatusMessage = "Your password has been changed.";

            return this.RedirectToPage();
        }
    }
}
