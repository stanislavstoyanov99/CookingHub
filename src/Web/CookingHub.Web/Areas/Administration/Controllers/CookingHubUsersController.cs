namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Common;
    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.CookingHubUsers;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CookingHubUsersController : AdministrationController
    {
        private readonly UserManager<CookingHubUser> cookingHubUserManager;
        private readonly ICookingHubUsersService cookingHubUsersService;
        private readonly RoleManager<ApplicationRole> roleManager;

        public CookingHubUsersController(
            UserManager<CookingHubUser> cookingHubUserManager,
            ICookingHubUsersService cookingHubUsersService,
            RoleManager<ApplicationRole> roleManager)
        {
            this.cookingHubUserManager = cookingHubUserManager;
            this.cookingHubUsersService = cookingHubUsersService;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> GetAll()
        {
            var users = await this.cookingHubUsersService
                .GetAllCookingHubUsersAsync<CookingHubUserDetailsViewModel>();

            return this.View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.cookingHubUserManager.FindByIdAsync(id);
            var isAdmin = await this.cookingHubUserManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);
            var isUser = await this.cookingHubUserManager.IsInRoleAsync(user, GlobalConstants.UserRoleName);

            var currUserRole = user.Roles.FirstOrDefault(x => x.UserId == id);
            var currUserRoleName = await this.roleManager.FindByIdAsync(currUserRole.RoleId);

            var cookingHubUserEditViewModel = new CookingHubUserEditViewModel
            {
                RoleId = currUserRole.RoleId,
                RoleName = currUserRoleName.Name,
            };

            cookingHubUserEditViewModel.RolesList = this.roleManager.Roles
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                })
                .ToList();

            if (currUserRoleName.Name == GlobalConstants.AdministratorRoleName && isAdmin == true)
            {
                cookingHubUserEditViewModel.RolesList
                    .Find(x => x.Text == GlobalConstants.AdministratorRoleName).Selected = true;
            }

            if (currUserRoleName.Name == GlobalConstants.UserRoleName && isUser == true)
            {
                cookingHubUserEditViewModel.RolesList
                    .Find(x => x.Text == GlobalConstants.UserRoleName).Selected = true;
            }

            return this.View(cookingHubUserEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CookingHubUserEditViewModel model, string id)
        {
            if (!this.ModelState.IsValid)
            {
                model.RolesList = this.roleManager.Roles
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                })
                .ToList();

                return this.View(model);
            }

            if (model.NewRole == model.RoleName)
            {
                model.RolesList = this.roleManager.Roles
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                })
                .ToList();

                return this.View(model);
            }

            var user = await this.cookingHubUserManager.FindByIdAsync(id);

            await this.cookingHubUserManager.RemoveFromRoleAsync(user, model.RoleName);

            await this.cookingHubUserManager.AddToRoleAsync(
                user,
                model.NewRole);

            return this.RedirectToAction("GetAll", "CookingHubUsers", new { area = "Administration" });
        }

        public async Task<IActionResult> Ban(string id)
        {
            var cookingHubUserToBan = await this.cookingHubUsersService
                .GetViewModelByIdAsync<CookingHubUserDetailsViewModel>(id);

            return this.View(cookingHubUserToBan);
        }

        [HttpPost]
        public async Task<IActionResult> Ban(CookingHubUserDetailsViewModel cookingHubUserDetailsViewModel)
        {
            await this.cookingHubUsersService.BanByIdAsync(cookingHubUserDetailsViewModel.Id);

            return this.RedirectToAction("GetAll", "CookingHubUsers", new { area = "Administration" });
        }

        public async Task<IActionResult> Unban(string id)
        {
            var cookingHubUserToUnban = await this.cookingHubUsersService
                .GetViewModelByIdAsync<CookingHubUserDetailsViewModel>(id);

            return this.View(cookingHubUserToUnban);
        }

        [HttpPost]
        public async Task<IActionResult> Unban(CookingHubUserDetailsViewModel cookingHubUserDetailsViewModel)
        {
            await this.cookingHubUsersService.UnbanByIdAsync(cookingHubUserDetailsViewModel.Id);

            return this.RedirectToAction("GetAll", "CookingHubUsers", new { area = "Administration" });
        }
    }
}
