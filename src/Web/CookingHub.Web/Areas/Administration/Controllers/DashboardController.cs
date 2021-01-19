namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.AdminDashboard;

    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly IDeletableEntityRepository<CookingHubUser> usersRepository;
        private readonly IDeletableEntityRepository<Recipe> recipesRepository;
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Review> reviewsRepository;
        private readonly IDeletableEntityRepository<ArticleComment> articleCommentRepository;
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public DashboardController(
            IDeletableEntityRepository<CookingHubUser> usersRepository,
            IDeletableEntityRepository<Recipe> recipesRepository,
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Review> reviewsRepository,
            IDeletableEntityRepository<ArticleComment> articleCommentRepository,
            IDeletableEntityRepository<Category> categoryRepository)
        {
            this.usersRepository = usersRepository;
            this.recipesRepository = recipesRepository;
            this.articlesRepository = articlesRepository;
            this.reviewsRepository = reviewsRepository;
            this.articleCommentRepository = articleCommentRepository;
            this.categoryRepository = categoryRepository;
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
                ArticleCommentsCount = this.articleCommentRepository.All().Count(),
                CategoriesCount = this.categoryRepository.All().Count(),
            };

            return this.View(statistics);
        }

        [HttpGet]
        public JsonResult FetchTopFive(string type)
        {
            switch (type)
            {
                case "Categories":
                    {
                        var categories = this.recipesRepository.All().GroupBy(g => g.Category.Name).Select(s => new { Key = s.Key, Count = s.Count() }).OrderByDescending(o => o.Count).ToList();
                        return this.Json(categories);
                    }

                case "Recipes":
                    {
                        var recipes = this.recipesRepository.All().OrderByDescending(o => o.Rate).Select(s => new { Key = s.Name, Rate = s.Rate }).Take(5).ToList();
                        return this.Json(recipes);
                    }

                case "Articles":
                    {
                        var articles = this.articlesRepository.All().OrderByDescending(o => o.ArticleComments.Count).Select(s => new { Key = s.Title, Count = s.ArticleComments.Count }).Take(5).ToList();
                        return this.Json(articles);
                    }

                default: return this.Json("No");
            }
        }
    }
}
