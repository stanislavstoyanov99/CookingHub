namespace CookingHub.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(CookingHubDbContext dbContext, IServiceProvider serviceProvider);
    }
}
