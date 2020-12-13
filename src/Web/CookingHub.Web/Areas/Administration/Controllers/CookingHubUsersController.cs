namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.CookingHubUsers;
    using CookingHub.Services.Data.Contracts;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CookingHubUsersController : AdministrationController
    {
        private readonly UserManager<CookingHubUser> cookingHubUser;
        private readonly ICookingHubUsersService cookingHubUsersService;
        private readonly RoleManager<ApplicationRole> roleManager;

        public CookingHubUsersController(
            UserManager<CookingHubUser> cookingHubUser,
            ICookingHubUsersService cookingHubUsersService,
            RoleManager<ApplicationRole> roleManager)
        {
            this.cookingHubUser = cookingHubUser;
            this.cookingHubUsersService = cookingHubUsersService;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> GetAll()
        {
            var users = await this.cookingHubUsersService.GetAllCookingHubUsersAsync<CookingHubUserDetailsViewModel>();

            return this.View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.cookingHubUser.FindByIdAsync(id);
            var admin = await this.cookingHubUser.IsInRoleAsync(user, "Administrator");
            var userrole = await this.cookingHubUser.IsInRoleAsync(user, "User");
            var model = new List<CookingHubUserEditViewModel>();

            foreach (var role in this.roleManager.Roles)
            {
                var cookingHubUserEditViewModel = new CookingHubUserEditViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };
                if (role.Name == "Administrator" && admin == true) { cookingHubUserEditViewModel.IsSelected = true; }
                if (role.Name == "User" && userrole == true) { cookingHubUserEditViewModel.IsSelected = true; }
                model.Add(cookingHubUserEditViewModel);
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(List<CookingHubUserEditViewModel> model, string id)
        {
            var user = await this.cookingHubUser.FindByIdAsync(id);

            var roles = await this.cookingHubUser.GetRolesAsync(user);
            var result = await this.cookingHubUser.RemoveFromRolesAsync(user, roles);

            result = await this.cookingHubUser.AddToRolesAsync(
                user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            return this.RedirectToAction("GetAll", "CookingHubUsers", new { area = "Administration" });
        }

        public async Task<IActionResult> Ban(string id)
        {
            var cookingHubUserToDelete = await this.cookingHubUsersService.GetViewModelByIdAsync<CookingHubUserDetailsViewModel>(id);

            return this.View(cookingHubUserToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Ban(CookingHubUserDetailsViewModel cookingHubUserDetailsViewModel)
        {
            await this.cookingHubUsersService.BanByIdAsync(cookingHubUserDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "CookingHubUsers", new { area = "Administration" });
        }

        public async Task<IActionResult> Unban(string id)
        {
            var cookingHubUserToDelete = await this.cookingHubUsersService.GetViewModelByIdAsync<CookingHubUserDetailsViewModel>(id);

            return this.View(cookingHubUserToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Unban(CookingHubUserDetailsViewModel cookingHubUserDetailsViewModel)
        {
            await this.cookingHubUsersService.UnbanByIdAsync(cookingHubUserDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "CookingHubUsers", new { area = "Administration" });
        }
    }
}

