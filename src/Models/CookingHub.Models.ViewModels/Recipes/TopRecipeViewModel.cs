namespace CookingHub.Models.ViewModels.Recipes
{
    using CookingHub.Data.Models;
    using CookingHub.Services.Mapping;

    public class TopRecipeViewModel : IMapFrom<Recipe>
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }

        public int Rate { get; set; }
    }
}
