namespace CookingHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Data.Common.Models;

    using static CookingHub.Data.Common.DataValidation.MessageValidation;

    public class Message : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        public virtual CookingHubUser User { get; set; }
    }
}
