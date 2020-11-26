namespace CookingHub.Web.Areas.Administration.Views.Faq
{
    using CookingHub.Web.Areas.Administration.Views.Shared;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class FaqNavPages : AdminNavPages
    {
        public static string CreateFaq => "CreateFaq";

        public static string GetAll => "GetAll";

        public static string CreateFaqNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateFaq);

        public static string GetAllNavClass(ViewContext viewContext) => PageNavClass(viewContext, GetAll);
    }
}
