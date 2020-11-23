

namespace CookingHub.Models.ViewModels.Articles
{
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Services.Mapping;

    using static CookingHub.Models.Common.ModelValidation;
    using static CookingHub.Models.Common.ModelValidation.ArticleValidation;
    using CookingHub.Data.Models;
    
    public class ArticlesEditViewModel : IMapFrom<Article>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = NameLengthError)]
        public string Title { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionError)]
        public string Description { get; set; }

        public int CategoryId { get; set; }
        [Required]
        public virtual Category Category { get; set; }

    }
}
