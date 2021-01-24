namespace CookingHub.Common.Attributes
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;

    using CookingHub.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class PasswordExpirationCheckAttribute : AuthorizationFilterAttribute, IAsyncAuthorizationFilter
    {
        private int maxPasswordAgeInDay;

        public PasswordExpirationCheckAttribute(int maxPasswordAgeInDay = int.MinValue)
        {
            this.maxPasswordAgeInDay = maxPasswordAgeInDay;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var isDefinedForControllerActions = false;
            var isDefinedForPageActions = false;

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var pageActionDescriptor = context.ActionDescriptor as PageActionDescriptor;

            if (controllerActionDescriptor != null)
            {
                isDefinedForControllerActions = controllerActionDescriptor
                    .MethodInfo
                    .GetCustomAttributes(inherit: true)
                    .Any(a => a.GetType().Equals(typeof(SkipPasswordExpirationCheckAttribute)));
            }

            if (pageActionDescriptor != null)
            {
                // TODO
            }

            if (!isDefinedForControllerActions)
            {
                //var user = context.HttpContext.User;

                //if (user != null && user.Identity.IsAuthenticated)
                //{
                //    var userManager = (UserManager<CookingHubUser>)context.HttpContext.RequestServices
                //        .GetService(typeof(UserManager<CookingHubUser>));
                //    var currUser = await userManager.GetUserAsync(user);
                //    var createdOn = currUser.CreatedOn;
                //    var timeSpan = DateTime.Today - createdOn;

                //    if (timeSpan.Days >= this.maxPasswordAgeInDay)
                //    {
                //        var infoMessage = $"Your account password expired and should be changed on every {this.maxPasswordAgeInDay} days";
                //        context.Result = new RedirectToActionResult(
                //            "ChangePassword", "Home", new ChangePasswordViewModel { Message = infoMessage });
                //    }
                //}
            }
        }
    }
}
