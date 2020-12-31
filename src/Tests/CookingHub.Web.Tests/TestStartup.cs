namespace CookingHub.Web.Tests
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    // TODO: add configuration for custom services
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            // Replace only your own custom services. The ASP.NET Core ones 
            // are already replaced by MyTested.AspNetCore.Mvc. 
            // services.Replace<IService, MockedService>();
        }
    }
}
