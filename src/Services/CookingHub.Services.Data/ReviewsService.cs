namespace CookingHub.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.Reviews;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class ReviewsService : IReviewsService
    {
        private const int TopReviewsFilter = 4;
        private readonly IDeletableEntityRepository<Review> reviewsRepository;
        private readonly IDeletableEntityRepository<Recipe> recipesRepository;

        public ReviewsService(
            IDeletableEntityRepository<Review> reviewsRepository,
            IDeletableEntityRepository<Recipe> recipesRepository)
        {
            this.reviewsRepository = reviewsRepository;
            this.recipesRepository = recipesRepository;
        }

        public async Task CreateAsync(CreateReviewInputModel createReviewInputModel)
        {
            var review = new Review
            {
                Title = createReviewInputModel.Title,
                RecipeId = createReviewInputModel.RecipeId,
                UserId = createReviewInputModel.UserId,
                Description = createReviewInputModel.Content,
                Rate = createReviewInputModel.Rate,
            };

            var doesExist = await this.reviewsRepository
                .All()
                .Where(x => x.UserId == createReviewInputModel.UserId && x.RecipeId == createReviewInputModel.RecipeId)
                .AnyAsync();

            if (doesExist)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ReviewAlreadyExists, review.Id));
            }
            else
            {
                await this.reviewsRepository.AddAsync(review);
                await this.reviewsRepository.SaveChangesAsync();

                var reviews = this.reviewsRepository
                    .All()
                    .Where(o => o.RecipeId == createReviewInputModel.RecipeId)
                    .ToList();
                var reviewsCount = reviews.Count;
                var oldrecipeRate = 0;

                foreach (var currReview in reviews)
                {
                    oldrecipeRate += currReview.Rate;
                }

                var newrating = oldrecipeRate / reviewsCount;

                var newrecipe = await this.recipesRepository
                    .All()
                    .FirstOrDefaultAsync(x => x.Id == createReviewInputModel.RecipeId);
                newrecipe.Rate = newrating;

                this.recipesRepository.Update(newrecipe);
                await this.reviewsRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            var review = await this.reviewsRepository
                .All()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ReviewNotFound, id));
            }

            this.reviewsRepository.Delete(review);
            await this.reviewsRepository.SaveChangesAsync();
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var reviewViewModel = await this.reviewsRepository
                .All()
                .Where(r => r.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (reviewViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ReviewNotFound, id));
            }

            return reviewViewModel;
        }

        public async Task<IEnumerable<TViewModel>> GetAll<TViewModel>(int recipeId)
        {
            var reviews = await this.reviewsRepository
                .All()
                .Where(r => r.RecipeId == recipeId)
                .To<TViewModel>()
                .ToListAsync();

            if (reviews.Count == 0)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ReviewsNotFound));
            }

            return reviews;
        }

        public async Task<IEnumerable<TViewModel>> GetTopReviews<TViewModel>()
        {
            var topReviews = await this.reviewsRepository
               .All()
               .Where(r => r.Rate >= TopReviewsFilter)
               .To<TViewModel>()
               .ToListAsync();

            return topReviews;
        }
    }
}
