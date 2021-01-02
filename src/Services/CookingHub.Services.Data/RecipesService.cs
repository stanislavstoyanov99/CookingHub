namespace CookingHub.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Data.Models.Enumerations;
    using CookingHub.Models.InputModels.AdministratorInputModels.Recipes;
    using CookingHub.Models.ViewModels.Recipes;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class RecipesService : IRecipesService
    {
        private const string AllPaginationFilter = "All";
        private readonly IDeletableEntityRepository<Recipe> recipesRepository;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly ICloudinaryService cloudinaryService;

        public RecipesService(
            IDeletableEntityRepository<Recipe> recipesRepository,
            IDeletableEntityRepository<Category> categoriesRepository,
            ICloudinaryService cloudinaryService)
        {
            this.recipesRepository = recipesRepository;
            this.categoriesRepository = categoriesRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<RecipeDetailsViewModel> CreateAsync(RecipeCreateInputModel recipeCreateInputModel, string userId)
        {
            if (!Enum.TryParse(recipeCreateInputModel.Difficulty, true, out Difficulty difficulty))
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.DifficultyInvalidType, recipeCreateInputModel.Difficulty));
            }

            var category = await this.categoriesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == recipeCreateInputModel.CategoryId);
            if (category == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.CategoryNotFound, recipeCreateInputModel.CategoryId));
            }

            var recipe = new Recipe
            {
                Name = recipeCreateInputModel.Name,
                Description = recipeCreateInputModel.Description,
                Category = category,
                UserId = userId,
                Ingredients = recipeCreateInputModel.Ingredients,
                PreparationTime = recipeCreateInputModel.PreparationTime,
                CookingTime = recipeCreateInputModel.CookingTime,
                Difficulty = difficulty,
            };

            bool doesRecipeExist = await this.recipesRepository
               .All()
               .AnyAsync(r => r.Name == recipe.Name);

            if (doesRecipeExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.RecipeAlreadyExists, recipe.Name));
            }

            var imageUrl = await this.cloudinaryService
                .UploadAsync(recipeCreateInputModel.Image, recipeCreateInputModel.Name + Suffixes.ArticleSuffix);
            recipe.ImagePath = imageUrl;

            await this.recipesRepository.AddAsync(recipe);
            await this.recipesRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<RecipeDetailsViewModel>(recipe.Id);

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var recipe = await this.recipesRepository
                .All()
                .FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.RecipeNotFound, id));
            }

            this.recipesRepository.Delete(recipe);
            await this.recipesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(RecipeEditViewModel recipeEditViewModel)
        {
            if (!Enum.TryParse(recipeEditViewModel.Difficulty, true, out Difficulty difficulty))
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.DifficultyInvalidType, recipeEditViewModel.Difficulty));
            }

            var recipe = await this.recipesRepository
                .All()
                .FirstOrDefaultAsync(r => r.Id == recipeEditViewModel.Id);

            if (recipe == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.RecipeNotFound, recipeEditViewModel.Id));
            }

            if (recipeEditViewModel.Image != null)
            {
                var newImageUrl = await this.cloudinaryService
                    .UploadAsync(recipeEditViewModel.Image, recipeEditViewModel.Name + Suffixes.RecipeSuffix);
                recipe.ImagePath = newImageUrl;
            }

            recipe.Name = recipeEditViewModel.Name;
            recipe.Description = recipeEditViewModel.Description;
            recipe.Ingredients = recipeEditViewModel.Ingredients;
            recipe.PreparationTime = recipeEditViewModel.PreparationTime;
            recipe.CookingTime = recipeEditViewModel.CookingTime;
            recipe.PortionsNumber = recipeEditViewModel.PortionsNumber;
            recipe.Difficulty = difficulty;
            recipe.CategoryId = recipeEditViewModel.CategoryId;

            this.recipesRepository.Update(recipe);
            await this.recipesRepository.SaveChangesAsync();
        }

        public IQueryable<TViewModel> GetAllRecipesAsQueryeable<TViewModel>()
        {
            var recipes = this.recipesRepository
                .All()
                .To<TViewModel>();

            return recipes;
        }

        public async Task<IEnumerable<TViewModel>> GetAllRecipesAsync<TViewModel>()
        {
            var recipes = await this.recipesRepository
               .All()
               .OrderBy(r => r.Name)
               .To<TViewModel>()
               .ToListAsync();

            return recipes;
        }

        public async Task<IEnumerable<TViewModel>> GetTopRecipesAsync<TViewModel>(int count = 0)
        {
            var topRecipes = await this.recipesRepository
               .All()
               .OrderByDescending(r => r.Rate)
               .To<TViewModel>()
               .Take(count)
               .ToListAsync();

            return topRecipes;
        }

        public IQueryable<TViewModel> GetAllRecipesByFilterAsQueryeable<TViewModel>(string categoryName = null)
        {
            var recipesByFilter = Enumerable.Empty<TViewModel>().AsQueryable();

            if (!string.IsNullOrEmpty(categoryName) && categoryName != AllPaginationFilter)
            {
                recipesByFilter = this.recipesRepository
                    .All()
                    .Where(x => x.Category.Name == categoryName)
                    .To<TViewModel>();
            }
            else
            {
                recipesByFilter = this.GetAllRecipesAsQueryeable<TViewModel>();
            }

            return recipesByFilter;
        }

        public async Task<TViewModel> GetRecipeAsync<TViewModel>(string name)
        {
            var recipe = await this.recipesRepository
                .All()
                .Where(r => r.Name == name)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (recipe == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.RecipeNameNotFound, name));
            }

            return recipe;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var recipeViewModel = await this.recipesRepository
                .All()
                .Where(r => r.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (recipeViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.RecipeNotFound, id));
            }

            return recipeViewModel;
        }

        public async Task<IEnumerable<TViewModel>> GetAllRecipesByUserId<TViewModel>(string userId)
        {
            var recipe = await this.recipesRepository
                .All()
                .Where(r => r.UserId == userId)
                .To<TViewModel>()
                .ToListAsync();

            return recipe;
        }
    }
}
