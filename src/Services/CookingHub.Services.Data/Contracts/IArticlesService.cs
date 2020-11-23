
namespace CookingHub.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.AdministratorInputModels.Articles;
    using CookingHub.Models.ViewModels.Articles;
    public interface IArticlesService : IBaseDataService
    {
        Task<ArticlesDetailsViewModel> CreateAsync(ArticleCreateInputModel articlesCreateInputModel);

        Task EditAsync(ArticlesEditViewModel articlesEditViewModel);

        Task<IEnumerable<TEntity>> GetAllArticlesAsync<TEntity>();

        Task<TViewModel> GetArticleAsync<TViewModel>(string title);
    }
}
