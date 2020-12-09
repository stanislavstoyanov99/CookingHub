namespace CookingHub.Models.ViewModels.Categories
{
    using System.Collections.Generic;

    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Services.Mapping;

    public class CategoryListingViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<RecentArticleListingViewModel> Articles { get; set; }
    }
}
