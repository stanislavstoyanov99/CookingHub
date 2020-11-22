namespace CookingHub.Models.ViewModels.Recipes
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    using CookingHub.Services.Mapping;

    using static CookingHub.Models.Common.ModelValidation;
    using static CookingHub.Models.Common.ModelValidation.RecipeValidation;
    using  CookingHub.Data.Models;
    using CookingHub.Data.Models.Enumerations;

    public class RecipesEditViewModel : IMapFrom<Recipe>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string Name { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionError)]
        public string Description { get; set; }
        [Required]
        [MaxLength(IngredientsMaxLength)]
        public string Ingredients { get; set; }

        public double PreparationTime { get; set; }

        public double CookingTime { get; set; }

        public int PortionsNumber { get; set; }

        [Required]
        public Difficulty Difficulty { get; set; }

        [Required]
        [MaxLength(ImagePathMaxLength)]
        public string ImagePath { get; set; }

        public int CategoryId { get; set; }
        [Required]
        public virtual Category Category { get; set; }

        
    }
}
