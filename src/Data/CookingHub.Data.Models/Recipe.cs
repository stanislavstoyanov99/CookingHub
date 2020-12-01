namespace CookingHub.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Data.Common.Models;
    using CookingHub.Data.Models.Enumerations;

    using static CookingHub.Data.Common.DataValidation.RecipeValidation;

    public class Recipe : BaseDeletableModel<int>
    {
        public Recipe()
        {
            this.Reviews = new HashSet<Review>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(IngredientsMaxLength)]
        public string Ingredients { get; set; }

        public int Rate { get; set; }

        public double PreparationTime { get; set; }

        public double CookingTime { get; set; }

        public int PortionsNumber { get; set; }

        [Required]
        public Difficulty Difficulty { get; set; }

        [Required]
        [MaxLength(ImagePathMaxLength)]
        public string ImagePath { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual CookingHubUser User { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
