namespace CookingHub.Models.ViewModels.Articles
{
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Services.Mapping;
    using CookingHub.Data.Models;

    using Ganss.XSS;

    using static CookingHub.Models.Common.ModelValidation;

    public class ArticleDetailsViewModel : IMapFrom<Article>
    {
        [Display(Name = IdDisplayName)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

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

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string SanitizedShortDescription => new HtmlSanitizer().Sanitize(this.ShortDescription);

        public string UserUsername { get; set; }
    }
}
