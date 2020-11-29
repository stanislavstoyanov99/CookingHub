namespace CookingHub.Models.ViewModels.Articles
{
    using System;

    using CookingHub.Data.Models;
    using CookingHub.Services.Mapping;

    public class RecentArticleListingViewModel : IMapFrom<Article>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImagePath { get; set; }

        public string UserUsername { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
