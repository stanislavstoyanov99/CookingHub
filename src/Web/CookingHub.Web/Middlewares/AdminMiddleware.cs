namespace CookingHub.Web.Middlewares
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Common;
    using CookingHub.Data.Models;
    using CookingHub.Data.Models.Enumerations;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    public class AdminMiddleware
    {
        private readonly RequestDelegate next;

        public AdminMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<CookingHubUser> userManager)
        {
            await this.SeedUserInRoles(userManager);
            await this.next(context);
        }

        private async Task SeedUserInRoles(UserManager<CookingHubUser> userManager)
        {
            if (!userManager.Users.Any(x => x.UserName == GlobalConstants.AdministratorUsername))
            {
                var user = new CookingHubUser
                {
                    UserName = GlobalConstants.AdministratorUsername,
                    Email = GlobalConstants.AdministratorEmail,
                    FullName = GlobalConstants.AdministratorFullName,
                    Gender = Gender.Male,
                };

                var result = await userManager.CreateAsync(user, GlobalConstants.AdministratorPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }
                else
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
