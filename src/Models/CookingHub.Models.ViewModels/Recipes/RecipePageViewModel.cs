namespace CookingHub.Models.ViewModels.Recipes
{
    using System.Collections.Generic;

    using CookingHub.Models.ViewModels.Categories;

    public class RecipePageViewModel
    {
        public PaginatedList<RecipeListingViewModel> RecipesListingViewModel { get; set; }

        public IEnumerable<CategoryListingViewModel> Categories { get; set; }

        public IEnumerable<RecipeListingViewModel> RecipesByCategory { get; set; }
    }
}
