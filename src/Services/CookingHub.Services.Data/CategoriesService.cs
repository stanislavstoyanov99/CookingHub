namespace CookingHub.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Models.InputModels.AdministratorInputModels.Categories;
    using CookingHub.Models.ViewModels.Categories;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<CategoryDetailsViewModel> CreateAsync(CategoryCreateInputModel categoryCreateInputModel)
        {
            var category = new Category
            {
                Name = categoryCreateInputModel.Name,
                Description = categoryCreateInputModel.Description,
            };

            bool doesCategoryExist = await this.categoriesRepository
                .All()
                .AnyAsync(c => c.Name == category.Name);
            if (doesCategoryExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.CategoryAlreadyExists, category.Name));
            }

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<CategoryDetailsViewModel>(category.Id);

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var category = await this.categoriesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.CategoryNotFound, id));
            }

            this.categoriesRepository.Delete(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(CategoryEditViewModel categoryEditViewModel)
        {
            var category = await this.categoriesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == categoryEditViewModel.Id);

            if (category == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.CategoryNotFound, categoryEditViewModel.Id));
            }

            category.Name = categoryEditViewModel.Name;
            category.Description = categoryEditViewModel.Description;

            this.categoriesRepository.Update(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllCategoriesAsync<TEntity>()
        {
            var categories = await this.categoriesRepository
               .All()
               .OrderBy(c => c.Name)
               .To<TEntity>()
               .ToListAsync();

            return categories;
        }

        public async Task<TViewModel> GetCategoryAsync<TViewModel>(string name)
        {
            var category = await this.categoriesRepository
                .All()
                .Where(c => c.Name == name)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var categoryViewModel = await this.categoriesRepository
                .All()
                .Where(c => c.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (categoryViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.CategoryNotFound, id));
            }

            return categoryViewModel;
        }
    }
}
