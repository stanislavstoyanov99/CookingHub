namespace CookingHub.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.AdministratorInputModels.Articles;
    using CookingHub.Models.ViewModels.Articles;

    public interface IArticlesService : IBaseDataService
    {
        Task<ArticleDetailsViewModel> CreateAsync(ArticleCreateInputModel articlesCreateInputModel, string userId);

        Task EditAsync(ArticleEditViewModel articlesEditViewModel, string userId);

        Task<IEnumerable<TViewModel>> GetAllArticlesAsync<TViewModel>();

        IQueryable<TViewModel> GetAllArticlesByCategoryNameAsQueryeable<TViewModel>(string categoryName);

        IQueryable<TViewModel> GetAllArticlesAsQueryeable<TViewModel>();

        Task<IEnumerable<TViewModel>> GetRecentArticlesAsync<TViewModel>(int count = 0);
    }
}
