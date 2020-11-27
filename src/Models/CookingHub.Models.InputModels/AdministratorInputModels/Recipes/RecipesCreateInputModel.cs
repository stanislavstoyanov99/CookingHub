namespace CookingHub.Models.InputModels.AdministratorInputModels.Recipes
{
    using CookingHub.Data.Models.Enumerations;
    using CookingHub.Models.ViewModels.Categories;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static CookingHub.Models.Common.ModelValidation;
    using static CookingHub.Models.Common.ModelValidation.CategoryValidation;

    public class RecipesCreateInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string Name { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionError)]
        public string Description { get; set; }

        public string Ingredients { get; set; }

        public double PreparationTime { get; set; }

        public double CookingTime { get; set; }

        [Range(0,12)]
        public int PortionsNumber { get; set; }

        [Required]
        public string Difficulty { get; set; }
        
        [Required]
        public string ImagePath { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<CategoryDetailsViewModel> Categories { get; set; }
    }
}
