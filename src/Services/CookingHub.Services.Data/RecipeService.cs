namespace CookingHub.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Models.InputModels.AdministratorInputModels.Recipes;
    using CookingHub.Models.ViewModels.Recipes;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class RecipeService : IRecipesService
    {
        private readonly IDeletableEntityRepository<Recipe> recipeRepository;

        public RecipeService(IDeletableEntityRepository<Recipe> recipeRepository)
        {
            this.recipeRepository = recipeRepository;
        }

        public async Task<RecipesDetailsViewModel> CreateAsync(RecipesCreateInputModel recipesCreateInputModel)
        {
            var recipe = new Recipe
            {
                Name = recipesCreateInputModel.Name,
                Description = recipesCreateInputModel.Description,
            };

            bool doesRecipeExist = await this.recipeRepository
               .All()
               .AnyAsync(c => c.Name == recipe.Name);

            if (doesRecipeExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.RecipeAlreadyExists, recipe.Name));
            }

            await this.recipeRepository.AddAsync(recipe);
            await this.recipeRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<RecipesDetailsViewModel>(recipe.Id);

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var category = await this.recipeRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.RecipeNotFound, id));
            }

            this.recipeRepository.Delete(category);
            await this.recipeRepository.SaveChangesAsync();
        }

        public async Task EditAsync(RecipesEditViewModel recipesEditViewModel)
        {
            var recipe = await this.recipeRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == recipesEditViewModel.Id);

            if (recipe == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.RecipeNotFound, recipesEditViewModel.Id));
            }

            recipe.Name = recipesEditViewModel.Name;
            recipe.Description = recipesEditViewModel.Description;

            this.recipeRepository.Update(recipe);
            await this.recipeRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllRecipesAsync<TEntity>()
        {
            var recipes = await this.recipeRepository
               .All()
               .OrderBy(c => c.Name)
               .To<TEntity>()
               .ToListAsync();

            return recipes;
        }

        public async Task<TViewModel> GetRecipeAsync<TViewModel>(string name)
        {
            var recipes = await this.recipeRepository
                .All()
                .Where(c => c.Name == name)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return recipes;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var recipesViewModel = await this.recipeRepository
                .All()
                .Where(c => c.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (recipesViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.RecipeNotFound, id));
            }

            return recipesViewModel;
        }
    }
}
