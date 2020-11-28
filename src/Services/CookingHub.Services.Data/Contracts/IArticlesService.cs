
namespace CookingHub.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.AdministratorInputModels.Articles;
    using CookingHub.Models.ViewModels.Articles;
    public interface IArticlesService : IBaseDataService
    {
        Task<ArticleDetailsViewModel> CreateAsync(ArticleCreateInputModel articlesCreateInputModel, string userId);

        Task EditAsync(ArticleEditViewModel articlesEditViewModel, string userId);

        Task<IEnumerable<TEntity>> GetAllArticlesAsync<TEntity>();

        Task<TViewModel> GetArticleAsync<TViewModel>(string title);

    }
}
