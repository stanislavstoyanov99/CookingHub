namespace CookingHub.Models.ViewModels.Recipes
{
    using CookingHub.Models.ViewModels.Reviews;

    public class RecipeDetailsPageViewModel
    {
        public RecipeDetailsViewModel Recipe { get; set; }

        public CreateReviewInputModel CreateReviewInputModel { get; set; }
    }
}
