namespace CookingHub.Models.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Services.Mapping;
    using CookingHub.Data.Models;


    using static CookingHub.Models.Common.ModelValidation;
    using static CookingHub.Models.Common.ModelValidation.CategoryValidation;

    public class CategoryEditViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string Name { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionError)]
        public string Description { get; set; }
    }
}
