namespace CookingHub.Models.ViewModels.Reviews
{
    using CookingHub.Data.Models;
    using CookingHub.Services.Mapping;

    using Ganss.XSS;

    public class ReviewListingViewModel : IMapFrom<Review>
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public int Rate { get; set; }

        public string UserUsername { get; set; }
    }
}
