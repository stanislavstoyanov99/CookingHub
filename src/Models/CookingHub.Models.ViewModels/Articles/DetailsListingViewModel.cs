namespace CookingHub.Models.ViewModels.Articles
{
    using System.Collections.Generic;

    using CookingHub.Models.ViewModels.Categories;

    public class DetailsListingViewModel
    {
        public ArticleListingViewModel ArticleListingViewModel { get; set; }

        public IEnumerable<CategoryListingViewModel> Categories { get; set; }

        public IEnumerable<RecentArticleListingViewModel> RecentArticles { get; set; }
    }
}
