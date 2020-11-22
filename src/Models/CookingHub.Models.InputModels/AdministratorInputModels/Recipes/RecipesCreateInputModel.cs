namespace CookingHub.Models.InputModels.AdministratorInputModels.Recipes
{
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

        public string PreparationTime { get; set; }

        public double CookingTime { get; set; }

        public int PortionsNumber { get; set; }

        [Required]
        public string Difficulty { get; set; }

        [Required]
        public string ImagePath { get; set; }

        public int CategoryId { get; set; }
    }
}
