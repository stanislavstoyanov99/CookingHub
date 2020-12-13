namespace CookingHub.Web.Areas.Administration.Views.Shared
{
    using System;
    using System.IO;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AdminNavPages
    {
        public static string Privacy => "Privacy";

        public static string About => "About";

        public static string Categories => "Categories";

        public static string Recipes => "Recipes";

        public static string Articles => "Articles";

        public static string CookingHubUsers => "CookingHubUsers";

        public static string PrivacyNavClass(ViewContext viewContext) => PageNavClass(viewContext, Privacy);

        public static string AboutNavClass(ViewContext viewContext) => PageNavClass(viewContext, About);

        public static string CategoriesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Categories);

        public static string RecipesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Recipes);

        public static string ArticlesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Articles);

        public static string CookingHubUsersNavClass(ViewContext viewContext) => PageNavClass(viewContext, CookingHubUsers);

        protected static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
