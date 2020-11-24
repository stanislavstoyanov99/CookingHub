namespace CookingHub.Models.ViewModels.Articles
{
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Services.Mapping;

    using static CookingHub.Models.Common.ModelValidation;
    using static CookingHub.Models.Common.ModelValidation.ArticleValidation;
    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.Categories;
    using System.Collections.Generic;

    public class ArticleEditViewModel : IMapFrom<Article>
    {
        public int Id { get; set; }

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
