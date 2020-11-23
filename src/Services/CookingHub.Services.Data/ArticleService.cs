
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
        private readonly IDeletableEntityRepository<Article> articleRepository;

        public ArticleService(IDeletableEntityRepository<Article> articleRepository)
        {
            this.articleRepository = articleRepository;
        }

        public async Task<ArticlesDetailsViewModel> CreateAsync(ArticleCreateInputModel articlesCreateInputModel)
        {
            var article = new Article
            {
                Title = articlesCreateInputModel.Title,
                Description = articlesCreateInputModel.Description,
            };
            bool doesArticleExist = await this.articleRepository
               .All()
               .AnyAsync(c => c.Title == article.Title);
            if (doesArticleExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.ArticleAlreadyExists, article.Title));
            }

            await this.articleRepository.AddAsync(article);
            await this.articleRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<ArticlesDetailsViewModel>(article.Id);

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var article = await this.articleRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == id);
            if (article == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.ArticleNotFound, id));
            }

            this.articleRepository.Delete(article);
            await this.articleRepository.SaveChangesAsync();
        }

        public async Task EditAsync(ArticlesEditViewModel articlesEditViewModel)
        {
            var article = await this.articleRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == articlesEditViewModel.Id);

            if (article == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.ArticleNotFound, articlesEditViewModel.Id));
            }

            article.Title = articlesEditViewModel.Title;
            article.Description = articlesEditViewModel.Description;

            this.articleRepository.Update(article);
            await this.articleRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllArticlesAsync<TEntity>()
        {
            var articles = await this.articleRepository
              .All()
              .OrderBy(c => c.Title)
              .To<TEntity>()
              .ToListAsync();

            return articles;
        }

        public async Task<TViewModel> GetArticleAsync<TViewModel>(string title)
        {
            var article = await this.articleRepository
                .All()
                .Where(c => c.Title == title)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return article;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var articlesViewModel = await this.articleRepository
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
