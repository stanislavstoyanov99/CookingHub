namespace CookingHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Data.Common.Models;

    using static CookingHub.Data.Common.DataValidation.ArticleValidation;

    public class Article : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public virtual Category Category { get; set; }

        public int CategoryId { get; set; }

        public string UserId { get; set; }

        public virtual CookingHubUser User { get; set; }
    }
}
