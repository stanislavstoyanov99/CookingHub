namespace CookingHub.Models.ViewModels.Recipes
{
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Data.Models;
    using CookingHub.Services.Mapping;
    using CookingHub.Data.Models.Enumerations;

    using Ganss.XSS;

    using static CookingHub.Models.Common.ModelValidation;
    using static CookingHub.Models.Common.ModelValidation.RecipeValidation;
    using System.Collections;
    using System.Collections.Generic;
    using CookingHub.Models.ViewModels.Reviews;

    public class RecipeDetailsViewModel : IMapFrom<Recipe>
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

        [Display(Name = PreparationTimeDisplayName)]
        public double PreparationTime { get; set; }

        [Display(Name = CookingTimeDisplayName)]
        public double CookingTime { get; set; }

        [Display(Name = PortionsNumberDisplayName)]
        public int PortionsNumber { get; set; }

        public int Rate { get; set; }

        public Difficulty Difficulty { get; set; }

        public string ImagePath { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string SanitizedShortDescription => new HtmlSanitizer().Sanitize(this.ShortDescription);

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string UserUsername { get; set; }

        public IEnumerable<ReviewDetailModel> Reviews { get; set; }
    }
}
