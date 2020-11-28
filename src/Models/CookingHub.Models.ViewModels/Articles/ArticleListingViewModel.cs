namespace CookingHub.Models.ViewModels.Articles
{
    using CookingHub.Data.Models;
    using CookingHub.Services.Mapping;

    using Ganss.XSS;
    using System;

    public class ArticleListingViewModel : IMapFrom<Article>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                var shortDescription = this.Description;
                return shortDescription.Length > 200
                        ? shortDescription.Substring(0, 200) + " ..."
                        : shortDescription;
            }
        }

        public string SanitizedShortDescription => new HtmlSanitizer().Sanitize(this.ShortDescription);

        public string UserUsername { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
