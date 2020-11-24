namespace CookingHub.Models.ViewModels.Recipes
{
    using CookingHub.Services.Mapping;
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Data.Models;
    using Ganss.XSS;
    using CookingHub.Data.Models.Enumerations;

    using static CookingHub.Models.Common.ModelValidation;
    public class RecipesDetailsViewModel : IMapFrom<Recipe>
    {
        [Display(Name = IdDisplayName)]
        public int Id { get; set; }

        public string Name { get; set; }

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
        public string Ingredients { get; set; }

        public double PreparationTime { get; set; }

        public double CookingTime { get; set; }

        public int PortionsNumber { get; set; }

        public Difficulty Difficulty { get; set; }
        [Required]
        public string ImagePath { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string SanitizedShortDescription => new HtmlSanitizer().Sanitize(this.ShortDescription);
    }
}
