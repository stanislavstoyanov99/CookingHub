
namespace CookingHub.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Models.InputModels.AdministratorInputModels.Articles;
    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class ArticleService : IArticlesService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public ArticleService(IDeletableEntityRepository<Article> articlesRepository, IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.articlesRepository = articlesRepository;
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<ArticlesDetailsViewModel> CreateAsync(ArticleCreateInputModel articleCreateInputModel, string userId)
        {
            var category = await this.categoriesRepository.All().FirstOrDefaultAsync(x => x.Id == articleCreateInputModel.CategoryId);
            if (category == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.CategoryNotFound, articleCreateInputModel.CategoryId));
            }

            var article = new Article
            {
                Title = articleCreateInputModel.Title,
                Description = articleCreateInputModel.Description,
                Category = category,
                UserId = userId,
            };
            bool doesArticleExist = await this.articlesRepository
               .All()
               .AnyAsync(c => c.Title == article.Title);
            if (doesArticleExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.ArticleAlreadyExists, article.Title));
            }

            await this.articlesRepository.AddAsync(article);
            await this.articlesRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<ArticlesDetailsViewModel>(article.Id);

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var article = await this.articlesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == id);
            if (article == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.ArticleNotFound, id));
            }

            this.articlesRepository.Delete(article);
            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(ArticleEditViewModel articlesEditViewModel, string userId)
        {
            var article = await this.articlesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == articlesEditViewModel.Id);

            if (article == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.ArticleNotFound, articlesEditViewModel.Id));
            }

            article.Title = articlesEditViewModel.Title;
            article.Description = articlesEditViewModel.Description;
            article.UserId = userId;

            this.articlesRepository.Update(article);
            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllArticlesAsync<TEntity>()
        {
            var articles = await this.articlesRepository
              .All()
              .OrderBy(c => c.Title)
              .To<TEntity>()
              .ToListAsync();

            return articles;
        }

        public async Task<TViewModel> GetArticleAsync<TViewModel>(string title)
        {
            var article = await this.articlesRepository
                .All()
                .Where(c => c.Title == title)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return article;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var articlesViewModel = await this.articlesRepository
                .All()
                .Where(c => c.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (articlesViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ArticleNotFound, id));
            }

            return articlesViewModel;
        }
    }
}
