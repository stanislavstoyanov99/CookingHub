namespace CookingHub.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class CookingHubUsersService : ICookingHubUsersService
    {
        private readonly IDeletableEntityRepository<CookingHubUser> cookingHubUsersRepository;

        public CookingHubUsersService(IDeletableEntityRepository<CookingHubUser> cookingHubUsersRepository)
        {
            this.cookingHubUsersRepository = cookingHubUsersRepository;
        }

        public async Task BanByIdAsync(string id)
        {
            var cookingHubUser = await this.cookingHubUsersRepository
                .All()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (cookingHubUser == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.CookingHubUserNotFound, id));
            }

            this.cookingHubUsersRepository.Delete(cookingHubUser);
            await this.cookingHubUsersRepository.SaveChangesAsync();
        }

        public async Task UnbanByIdAsync(string id)
        {
            var cookingHubUser = await this.cookingHubUsersRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (cookingHubUser == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.CookingHubUserNotFound, id));
            }

            this.cookingHubUsersRepository.Undelete(cookingHubUser);
            await this.cookingHubUsersRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TViewModel>> GetAllCookingHubUsersAsync<TViewModel>()
        {
            var users = await this.cookingHubUsersRepository
              .AllWithDeleted()
              .To<TViewModel>()
              .ToListAsync();

            return users;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(string id)
        {
            var cookingHubUserViewModel = await this.cookingHubUsersRepository
                .AllWithDeleted()
                .Where(u => u.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (cookingHubUserViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.CookingHubUserNotFound, id));
            }

            return cookingHubUserViewModel;
        }
    }
}
