﻿namespace CookingHub.Web.Areas.Administration.Views.Shared
{
    using System;
    using System.IO;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AdminNavPages
    {
        public static string Privacy => "Privacy";

        public static string About => "About";

        public static string PrivacyNavClass(ViewContext viewContext) => PageNavClass(viewContext, Privacy);

        public static string AboutNavClass(ViewContext viewContext) => PageNavClass(viewContext, About);

        protected static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}