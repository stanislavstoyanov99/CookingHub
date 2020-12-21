namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Linq;

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
    }
}
