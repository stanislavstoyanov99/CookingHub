namespace CookingHub.Web.Hubs
{
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.Chat;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync(
                "NewMessage",
                new Message { User = this.Context.User.Identity.Name, Text = message });
        }
    }
}
