namespace CookingHub.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Models.InputModels.AdministratorInputModels.Recipes;
    using CookingHub.Models.InputModels.Reviews;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    public class ReviewsService : IReviewsService 
    {
        private readonly IDeletableEntityRepository<Review> reviewsRepository;
        private readonly IDeletableEntityRepository<Recipe> recipesRepository;

        public ReviewsService(IDeletableEntityRepository<Review> reviewsRepository ,IDeletableEntityRepository<Recipe> recipesRepository)
        {
            this.reviewsRepository = reviewsRepository;
            this.recipesRepository = recipesRepository;
        }

        public async Task CreateAsync(CreateReviewInputModel createReviewInputModel)
        {
            var review = new Review
            {
                RecipeId = createReviewInputModel.RecipeId,
                UserId = createReviewInputModel.UserId,
                Description = createReviewInputModel.Content,
                Rate = createReviewInputModel.Rate,
            };

            await this.reviewsRepository.AddAsync(review);
            await this.reviewsRepository.SaveChangesAsync();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TViewModel>> GetAll<TViewModel>(int recipeId)
        {
            var reviews = await this.reviewsRepository
           .All()
           .Where(o => o.RecipeId == recipeId)
           .To<TViewModel>()
           .ToListAsync();
            return reviews;
        }
    }
}
