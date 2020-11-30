namespace CookingHub.Web.Areas.Administration.Controllers
{
    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.AdminDashboard;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System;
   
    using Newtonsoft.Json;
    using System.Security.Cryptography.X509Certificates;

    public class DashboardController : AdministrationController
    {
        private readonly IDeletableEntityRepository<CookingHubUser> usersRepository;
        private readonly IDeletableEntityRepository<Recipe> recipesRepository;
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Review> reviewsRepository;

        public DashboardController(IDeletableEntityRepository<CookingHubUser> usersRepository,IDeletableEntityRepository<Recipe> recipesRepository, IDeletableEntityRepository<Article> articlesRepository, IDeletableEntityRepository<Review> reviewsRepository)
        {
            this.usersRepository = usersRepository;
            this.recipesRepository = recipesRepository;
            this.articlesRepository = articlesRepository;
            this.reviewsRepository = reviewsRepository;
        }
        
        public IActionResult Index()
        {
            var statistics = new DashboardContentModel()
            {
                RecipesCount = this.recipesRepository.All().Count(),
                ArticlesCount = this.articlesRepository.All().Count(),
                ReviewsCount = this.reviewsRepository.All().Count(),
                RegisteredUsers = this.usersRepository.All().Count(),
                Admins = this.usersRepository.All().Where(x => x.UserName == "Admin").Count(),
            };

            return this.View(statistics);
        }
    }
}
