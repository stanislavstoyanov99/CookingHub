namespace CookingHub.Web.Tests
{
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.Primitives;

    public class TestActionDescriptorChangeProvider : IActionDescriptorChangeProvider
    {
        public IChangeToken GetChangeToken()
        {
            return new CompositeChangeToken(new IChangeToken[0]);
        }
    }
}
