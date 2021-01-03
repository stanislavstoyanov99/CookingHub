namespace CookingHub.Web.Tests.Controllers
{
    using CookingHub.Web.Controllers;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTests
    {
        [Fact]
        public void ChatShouldReturnView()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Chat())
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();
    }
}
