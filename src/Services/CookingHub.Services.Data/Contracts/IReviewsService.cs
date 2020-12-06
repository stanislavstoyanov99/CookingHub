namespace CookingHub.Services.Data.Contracts
{
    using CookingHub.Models.InputModels.Reviews;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;


    public interface IReviewsService : IBaseDataService
    {
        public Task CreateAsync(CreateReviewInputModel createReviewInputModel);

        public Task<IEnumerable<TViewModel>> GetAll<TViewModel>(int recipeId);
    }
}
