namespace CookingHub.Models.ViewModels.Recipes
{
    using System.Collections.Generic;

    using CookingHub.Models.ViewModels.Categories;

    public class RecipePageViewModel
    {
        public PaginatedList<RecipeListingViewModel> RecipesPaginated { get; set; }

        public IEnumerable<CategoryListingViewModel> Categories { get; set; }
    }
}
