namespace CookingHub.Web.Areas.Administration.Views.Recipes
{
    using CookingHub.Web.Areas.Administration.Views.Shared;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class RecipesNavPages : AdminNavPages
    {
        public static string CreateRecipe => "CreateRecipe";

        public static string GetAll => "GetAll";

        public static string CreateRecipeNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateRecipe);

        public static string GetAllNavClass(ViewContext viewContext) => PageNavClass(viewContext, GetAll);
    }
}
