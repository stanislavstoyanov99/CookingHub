namespace CookingHub.Models.ViewModels.Reviews
{
    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.Recipes;
    using CookingHub.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Text;

   public class ReviewDetailModel : IMapFrom<Review>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Rate { get; set; }

        public int RecipeId { get; set; }

        public RecipeDetailsViewModel Recipe { get; set; }

        public string UserId { get; set; }

        public string UserUsername { get; set; }

    }
}
