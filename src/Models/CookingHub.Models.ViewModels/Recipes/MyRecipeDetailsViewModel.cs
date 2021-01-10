namespace CookingHub.Models.ViewModels.Recipes
{
    using System.Collections.Generic;

    public class MyRecipeDetailsViewModel
    {
        public IEnumerable<RecipeDetailsViewModel> Recipes { get; set; }

        public RecipeEditViewModel RecipeEditViewModel { get; set; }
    }
}
