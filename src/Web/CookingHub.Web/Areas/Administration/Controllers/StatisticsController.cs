namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Linq;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IDeletableEntityRepository<CookingHubUser> usersRepository;
        private readonly IDeletableEntityRepository<Recipe> recipesRepository;
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Review> reviewsRepository;
        private readonly IDeletableEntityRepository<ArticleComment> commentsRepository;

        public StatisticsController(
            IDeletableEntityRepository<CookingHubUser> usersRepository,
            IDeletableEntityRepository<Recipe> recipesRepository,
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Review> reviewsRepository,
            IDeletableEntityRepository<ArticleComment> commentsRepository)
        {
            this.usersRepository = usersRepository;
            this.recipesRepository = recipesRepository;
            this.articlesRepository = articlesRepository;
            this.reviewsRepository = reviewsRepository;
            this.commentsRepository = commentsRepository;
        }

        [HttpGet]
        public ActionResult<StatisticsResponseModel> Get()
        {
            var registeredUsersArray = new int[12];
            for (int i = 0; i < registeredUsersArray.Length; i++)
            {
                registeredUsersArray[i] = this.usersRepository.All().Where(o => o.CreatedOn.Month == (i + 1)).Count();
            }

            var model = new StatisticsResponseModel()
            {
                recipesCount = this.recipesRepository.All().Count(),
                articlesCount = this.articlesRepository.All().Count(),
                reviewedrecipesCount = this.recipesRepository.All().Where(x => x.Reviews.Count >= 0).Count(),
                reviewsCount = this.reviewsRepository.All().Count(),
                commentsCount = this.commentsRepository.All().Count(),
            };
            model.registeredUsers = registeredUsersArray;
            return model;
        }
    }
}
