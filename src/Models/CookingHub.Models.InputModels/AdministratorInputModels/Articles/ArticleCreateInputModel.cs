

namespace CookingHub.Models.InputModels.AdministratorInputModels.Articles
{
    using CookingHub.Models.ViewModels.Categories;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    using static CookingHub.Models.Common.ModelValidation;
    using static CookingHub.Models.Common.ModelValidation.ArticleValidation;
    public class ArticleCreateInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = NameLengthError)]
        public string Title { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionError)]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<CategoryDetailsViewModel> Categories { get; set; }
    }
}
