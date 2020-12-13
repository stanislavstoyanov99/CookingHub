namespace CookingHub.Models.ViewModels.Home
{
    using System.Collections.Generic;

    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Models.ViewModels.Recipes;

    public class HomePageViewModel
    {
        public IEnumerable<ArticleListingViewModel> RecentArticles { get; set; }

        public IEnumerable<RecipeListingViewModel> TopRecipes { get; set; }

        public IEnumerable<GalleryViewModel> Gallery { get; set; }
    }
}
