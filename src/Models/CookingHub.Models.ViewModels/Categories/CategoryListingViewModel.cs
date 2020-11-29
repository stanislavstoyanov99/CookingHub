namespace CookingHub.Models.ViewModels.Categories
{
    using CookingHub.Data.Models;
    using CookingHub.Services.Mapping;

    public class CategoryListingViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
