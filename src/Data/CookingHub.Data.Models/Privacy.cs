namespace CookingHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Data.Common.Models;

    using static CookingHub.Data.Common.DataValidation.PrivacyValidation;

    public class Privacy : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(ContentPageMaxLength)]
        public string PageContent { get; set; }
    }
}
