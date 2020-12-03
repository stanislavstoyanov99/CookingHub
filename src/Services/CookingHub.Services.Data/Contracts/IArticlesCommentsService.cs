namespace CookingHub.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IArticlesCommentsService 
    {
        Task CreateAsync(int articleId, string userId, string content, int? parentId = null);

        Task<bool> IsInArticlesId(int commentId, int articleId);
    }
}
