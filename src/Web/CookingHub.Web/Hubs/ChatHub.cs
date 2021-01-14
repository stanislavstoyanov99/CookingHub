namespace CookingHub.Web.Hubs
{
    using System;
    using System.Threading.Tasks;

    using CookingHub.Data.Models;
    using CookingHub.Models.ViewModels.Chat;
    using CookingHub.Services.Data.Common;
    using CookingHub.Services.Data.Contracts;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;

    using static CookingHub.Models.Common.ModelValidation;
    using static CookingHub.Models.Common.ModelValidation.MessageValidation;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService chatService;
        private readonly UserManager<CookingHubUser> userManager;

        public ChatHub(IChatService chatService, UserManager<CookingHubUser> userManager)
        {
            this.chatService = chatService;
            this.userManager = userManager;
        }

        public async Task GetMessages()
        {
            try
            {
                var messages = await this.chatService.GetAllMessagesAsync<MessageViewModel>();
                await this.Clients.All.SendAsync("showMessages", messages);
            }
            catch (Exception ex)
            {
                await this.Clients.Caller.SendAsync("onError", "Error:" + ex.Message);
            }
        }

        public async Task SendMessage(string message)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(this.Context.User);

                if (user == null)
                {
                    throw new NullReferenceException(UserError);
                }

                if (string.IsNullOrEmpty(message.Trim()))
                {
                    throw new ArgumentException(EmptyFieldLengthError);
                }

                if (message.Trim().Length > ContentMaxLength)
                {
                    throw new ArgumentException(string.Format(ContentMaxLengthError, ContentMaxLength));
                }

                // Create and save message in database
                var messageInputModel = new MessageInputModel
                {
                    Content = new HtmlSanitizer().Sanitize(message),
                    UserId = user.Id,
                    UserName = user.UserName,
                };

                if (string.IsNullOrEmpty(messageInputModel.Content))
                {
                    throw new ArgumentException(ExceptionMessages.InvalidMessageError);
                }

                await this.chatService.CreateAsync(messageInputModel);
                var messages = await this.chatService.GetAllMessagesAsync<MessageViewModel>();

                // Broadcast the messages
                await this.Clients.All.SendAsync("receiveMessages", messages);
            }
            catch (Exception ex)
            {
                await this.Clients.Caller.SendAsync("onError", ex.Message);
            }
        }

        public async Task RemoveMessage(int messageId)
        {
            try
            {
                await this.chatService.DeleteByIdAsync(messageId);
                var messages = await this.chatService.GetAllMessagesAsync<MessageViewModel>();

                await this.Clients.All.SendAsync("deleteMessage", messages);
            }
            catch (Exception ex)
            {
                await this.Clients.Caller.SendAsync("onError", ex.Message);
            }
        }
    }
}
