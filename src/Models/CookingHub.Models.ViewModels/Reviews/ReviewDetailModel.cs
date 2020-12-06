namespace CookingHub.Models.ViewModels.Reviews
{
    using CookingHub.Data.Models;
    using CookingHub.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Text;

    class ReviewDetailModel : IMapFrom<Review>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Rate { get; set; }

        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }

        public string UserId { get; set; }

        public virtual CookingHubUser User { get; set; }

        public virtual ICollection<ReviewComment> ReviewComments { get; set; }
    }
}
