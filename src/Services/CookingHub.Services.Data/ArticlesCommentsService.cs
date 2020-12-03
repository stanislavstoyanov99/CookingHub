namespace CookingHub.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class ArticlesCommentsService : IArticlesCommentsService
    {
        private readonly IDeletableEntityRepository<ArticleComment> articlesCommentsRepository;

        public ArticlesCommentsService(IDeletableEntityRepository<ArticleComment> articlesCommentsrepository)
        {
            this.articlesCommentsRepository = articlesCommentsrepository;
        }

        public async Task CreateAsync(int articleId, string userId, string content, int? parentId = null)
        {
            var articleComment = new ArticleComment
            {
                ArticleId = articleId,
                UserId = userId,
                Content = content,
                ParentId = parentId,
            };

            bool doesArticleCommentExist = await this.articlesCommentsRepository
                .All()
                .AnyAsync(x => x.ArticleId == articleComment.ArticleId && x.UserId == userId && x.Content == content);
            if (doesArticleCommentExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.ArticleCommentAlreadyExists, articleComment.ArticleId, articleComment.Content));
            }

            await this.articlesCommentsRepository.AddAsync(articleComment);
            await this.articlesCommentsRepository.SaveChangesAsync();
        }

        public async Task<bool> IsInArticlesId(int commentId, int articleId)
        {
            var commentArticleId = await this.articlesCommentsRepository
                .All()
                .Where(x => x.Id == commentId)
                .Select(x => x.ArticleId)
                .FirstOrDefaultAsync();

            return commentArticleId == articleId;
        }
    }
}
