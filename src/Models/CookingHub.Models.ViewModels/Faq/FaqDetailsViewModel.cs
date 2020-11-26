namespace CookingHub.Models.ViewModels.Faq
{
    using Ganss.XSS;

    using CookingHub.Data.Models;
    using CookingHub.Services.Mapping;

    public class FaqDetailsViewModel : IMapFrom<FaqEntry>
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string SanitizedAnswer => new HtmlSanitizer().Sanitize(this.Answer);
    }
}
