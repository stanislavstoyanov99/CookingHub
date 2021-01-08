namespace CookingHub.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookingHub.Data.Common.Repositories;
    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.Chat;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using CookingHub.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class ChatService : IChatService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;

        public ChatService(IDeletableEntityRepository<Message> messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }

        public async Task<MessageViewModel> CreateAsync(MessageInputModel messageCreateInputModel)
        {
            var message = new Message
            {
                UserId = messageCreateInputModel.UserId,
                UserName = messageCreateInputModel.UserName,
                Content = messageCreateInputModel.Content,
            };

            await this.messagesRepository.AddAsync(message);
            await this.messagesRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<MessageViewModel>(message.Id);

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var message = await this.messagesRepository
                .All()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.MessageNotFound, id));
            }

            this.messagesRepository.Delete(message);
            await this.messagesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TViewModel>> GetAllMessagesAsync<TViewModel>()
        {
            var messages = await this.messagesRepository
              .All()
              .To<TViewModel>()
              .ToListAsync();

            return messages;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var messageViewModel = await this.messagesRepository
                .All()
                .Where(m => m.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (messageViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.MessageNotFound, id));
            }

            return messageViewModel;
        }
    }
}
