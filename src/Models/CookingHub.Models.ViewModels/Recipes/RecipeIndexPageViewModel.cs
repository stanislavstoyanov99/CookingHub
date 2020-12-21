namespace CookingHub.Models.ViewModels.Recipes
{
    using System.Collections.Generic;

    using CookingHub.Models.ViewModels.Categories;
    using CookingHub.Models.ViewModels.Reviews;

    public class RecipeIndexPageViewModel
    {
        public PaginatedList<RecipeListingViewModel> RecipesPaginated { get; set; }

        public IEnumerable<CategoryListingViewModel> Categories { get; set; }

        public IEnumerable<ReviewListingViewModel> Reviews { get; set; }
    }
}
