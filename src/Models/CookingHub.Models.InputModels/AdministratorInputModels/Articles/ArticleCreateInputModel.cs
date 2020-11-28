namespace CookingHub.Models.InputModels.AdministratorInputModels.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Models.ViewModels.Categories;

    using static CookingHub.Models.Common.ModelValidation;
    using static CookingHub.Models.Common.ModelValidation.ArticleValidation;

    public class ArticleCreateInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = TitleLengthError)]
        public string Title { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionLengthError)]
        public string Description { get; set; }

        [Display(Name = CategoryDisplayName)]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryDetailsViewModel> Categories { get; set; }
    }
}
