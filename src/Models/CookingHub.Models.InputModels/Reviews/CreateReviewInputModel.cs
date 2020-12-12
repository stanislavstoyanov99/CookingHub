namespace CookingHub.Models.InputModels.Reviews
{
    public class CreateReviewInputModel
    {
        public string Title { get; set; }

        public int RecipeId { get; set;}

        public int Rate { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }
    }
}
