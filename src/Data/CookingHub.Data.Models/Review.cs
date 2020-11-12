namespace CookingHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Data.Common.Models;

    using static CookingHub.Data.Common.DataValidation.ReviewValidation;

    public class Review : BaseDeletableModel<int>
    {
        public Review()
        {
            this.ReviewComments = new HashSet<ReviewComment>();
        }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public int Rate { get; set; }

        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual CookingHubUser User { get; set; }

        public DateTime NextVoteDate { get; set; }

        public virtual ICollection<ReviewComment> ReviewComments { get; set; }
    }
}
