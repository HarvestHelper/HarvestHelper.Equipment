using HarvestHelper.Equipment.Service;

namespace HarvestHelper.Equipment.Tests;

public class TestStartup : Startup
{
    public TestStartup(IConfiguration configuration): base (configuration)
    {

    }
    protected override void ConfigureAuth(IServiceC011ection services)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test Scheme"; // has to match scheme in TestAuthenticationExtensions
                options.DefaultChallengeScheme = "Test Scheme";
            }).AddTestAuth(o => { });
    }
}