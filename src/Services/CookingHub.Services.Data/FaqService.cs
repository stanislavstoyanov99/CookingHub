namespace CookingHub.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Models.InputModels.AdministratorInputModels.Faq;
    using CookingHub.Models.ViewModels.Faq;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class FaqService : IFaqService
    {
        private readonly IDeletableEntityRepository<FaqEntry> faqEntriesRepository;

        public FaqService(IDeletableEntityRepository<FaqEntry> faqEntriesRepository)
        {
            this.faqEntriesRepository = faqEntriesRepository;
        }

        public async Task<FaqDetailsViewModel> CreateAsync(FaqCreateInputModel faqCreateInputModel)
        {
            var faq = new FaqEntry
            {
                Question = faqCreateInputModel.Question,
                Answer = faqCreateInputModel.Answer,
            };

            bool doesFaqExist = await this.faqEntriesRepository
                .All()
                .AnyAsync(x => x.Question == faqCreateInputModel.Question && x.Answer == faqCreateInputModel.Answer);

            if (doesFaqExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.FaqAlreadyExists, faqCreateInputModel.Question, faqCreateInputModel.Answer));
            }

            await this.faqEntriesRepository.AddAsync(faq);
            await this.faqEntriesRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<FaqDetailsViewModel>(faq.Id);

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var faq = await this.faqEntriesRepository.All().FirstOrDefaultAsync(fe => fe.Id == id);

            if (faq == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.FaqNotFound, id));
            }

            this.faqEntriesRepository.Delete(faq);
            await this.faqEntriesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(FaqEditViewModel faqEditViewModel)
        {
            var faq = await this.faqEntriesRepository.All().FirstOrDefaultAsync(fe => fe.Id == faqEditViewModel.Id);

            if (faq == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.FaqNotFound, faqEditViewModel.Id));
            }

            faq.Answer = faqEditViewModel.Answer;
            faq.Question = faqEditViewModel.Question;

            this.faqEntriesRepository.Update(faq);
            await this.faqEntriesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllFaqsAsync<TEntity>()
        {
            var faqs = await this.faqEntriesRepository
               .All()
               .To<TEntity>()
               .ToListAsync();

            return faqs;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var faqViewModel = await this.faqEntriesRepository
              .All()
              .Where(fe => fe.Id == id)
              .To<TViewModel>()
              .FirstOrDefaultAsync();

            if (faqViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.FaqNotFound, id));
            }

            return faqViewModel;
        }
    }
}
