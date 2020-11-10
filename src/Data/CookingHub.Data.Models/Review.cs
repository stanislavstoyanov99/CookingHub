namespace CookingHub.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Data.Common.Models;

    using static CookingHub.Data.Common.DataValidation.ReviewValidation;

    public class Review : BaseDeletableModel<int>
    {
        public Review()
        {
            //this.ReviewComments = new HashSet<ReviewComment>();
        }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public int UserId { get; set; }

        public int Rate { get; set; }

        public int RecepieID { get; set; }

        public virtual CookingHubUser User { get; set; }

        public virtual DateTime PostDate { get; set; }

        //public virtual ICollection<ReviewComment> ReviewComments { get; set; }
    }
}