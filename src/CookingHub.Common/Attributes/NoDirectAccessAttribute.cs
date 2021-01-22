namespace CookingHub.Common.Attributes
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var canAcess = false;

            // It is not the best practice, should be changed in further state
            var referer = filterContext.HttpContext.Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(referer))
            {
                var refererUri = new UriBuilder(referer).Uri;
                var req = filterContext.HttpContext.Request;

                if (req.Host.Host == refererUri.Host && req.Host.Port == refererUri.Port && req.Scheme == refererUri.Scheme)
                {
                    canAcess = true;
                }
            }

            if (!canAcess)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Home", action = "Index", area = string.Empty }));
            }
        }
    }
}
