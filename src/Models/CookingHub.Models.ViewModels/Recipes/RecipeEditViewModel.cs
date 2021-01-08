namespace CookingHub.Models.ViewModels.Recipes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Data.Models;
    using CookingHub.Services.Mapping;
    using CookingHub.Models.ViewModels.Categories;

    using Microsoft.AspNetCore.Http;

    using static CookingHub.Models.Common.ModelValidation;
    using static CookingHub.Models.Common.ModelValidation.RecipeValidation;

    public class RecipeEditViewModel : IMapFrom<Recipe>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string Name { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionError)]
        public string Description { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(IngredientsMaxLength, MinimumLength = IngredientsMinLength, ErrorMessage = IngredientsError)]
        public string Ingredients { get; set; }

        [Display(Name = PreparationTimeDisplayName)]
        [Range(PreparationTimeMinLength, PreparationTimeMaxLength)]
        public double PreparationTime { get; set; }

        [Display(Name = CookingTimeDisplayName)]
        [Range(CookingTimeMinLength, CookingTimeMaxLength)]
        public double CookingTime { get; set; }

        [Display(Name = PortionsNumberDisplayName)]
        [Range(PortionsMinLength, PortionsMaxLength)]
        public int PortionsNumber { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        public string Difficulty { get; set; }

        public string ImagePath { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [Display(Name = nameof(Category))]
        public int CategoryId { get; set; }
     
        public IEnumerable<CategoryDetailsViewModel> Categories { get; set; }
    }
}
