namespace CookingHub.Models.ViewModels.ArticleComments
{
    public class CreateArticleCommentInputModel
    {
        public int ArticleId { get; set; }

        public int ParentId { get; set; }

        public string Content { get; set; }
    }
}
