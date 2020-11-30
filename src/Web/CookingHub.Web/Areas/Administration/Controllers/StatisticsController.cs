using CookingHub.Data.Common.Repositories;
using CookingHub.Data.Models;
using CookingHub.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookingHub.Web.Areas.Administration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IDeletableEntityRepository<CookingHubUser> usersRepository;
        private readonly IDeletableEntityRepository<Recipe> recipesRepository;
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Review> reviewsRepository;

        public StatisticsController(IDeletableEntityRepository<CookingHubUser> usersRepository, IDeletableEntityRepository<Recipe> recipesRepository, IDeletableEntityRepository<Article> articlesRepository, IDeletableEntityRepository<Review> reviewsRepository)
        {
            this.usersRepository = usersRepository;
            this.recipesRepository = recipesRepository;
            this.articlesRepository = articlesRepository;
            this.reviewsRepository = reviewsRepository;
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
            };
            model.registeredUsers = registeredUsersArray;
            return model;
        }
    }
}
