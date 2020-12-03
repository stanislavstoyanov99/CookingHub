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

    public class ArticleCommentsService : IArticleCommentsService
    {
        private readonly IDeletableEntityRepository<ArticleComment> articleCommentsRepository;

        public ArticleCommentsService(IDeletableEntityRepository<ArticleComment> articleCommentsrepository)
        {
            this.articleCommentsRepository = articleCommentsrepository;
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

            bool doesArticleCommentExist = await this.articleCommentsRepository
                .All()
                .AnyAsync(x => x.ArticleId == articleComment.ArticleId && x.UserId == userId && x.Content == content);
            if (doesArticleCommentExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.ArticleCommentAlreadyExists, articleComment.ArticleId, articleComment.Content));
            }

            await this.articleCommentsRepository.AddAsync(articleComment);
            await this.articleCommentsRepository.SaveChangesAsync();
        }

        public async Task<bool> IsInArticleId(int commentId, int articleId)
        {
            var commentArticleId = await this.articleCommentsRepository
                .All()
                .Where(x => x.Id == commentId)
                .Select(x => x.ArticleId)
                .FirstOrDefaultAsync();

            return commentArticleId == articleId;
        }
    }
}
