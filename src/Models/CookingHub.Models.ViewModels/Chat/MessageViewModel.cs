namespace CookingHub.Models.ViewModels.Chat
{
    using System;

    using CookingHub.Data.Models;
    using CookingHub.Services.Mapping;

    public class MessageViewModel : IMapFrom<Message>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string UserUsername { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
