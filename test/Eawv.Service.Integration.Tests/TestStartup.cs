// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Eawv.Service.Integration.Tests.Mocks;
using Eawv.Service.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.MalwareScanner.Services;
using Voting.Lib.Testing.Mocks;

namespace Eawv.Service.Integration.Tests;

public class TestStartup : Startup
{
    public TestStartup(IConfiguration configuration)
        : base(configuration)
    {
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services
            .AddVotingLibIamMocks()
            .RemoveHostedServices()
            .RemoveAll<ITenantService>()
            .AddTransient<ITenantService, TenantServiceMock>()
            .RemoveAll<IUserService>()
            .AddTransient<IUserService, UserServiceMock>()
            .RemoveAll<INotificationService>()
            .AddTransient<NotificationServiceMock>()
            .AddTransient<INotificationService, NotificationServiceMock>(sp => sp.GetRequiredService<NotificationServiceMock>())
            .AddMock<IPdfService, MockPdfRenderer>()
            .AddMock<IMalwareScannerService, MalwareScannerMock>()
            .AddMockedClock();
    }

    protected override void ConfigureAuthentication(AuthenticationBuilder builder)
        => builder.AddMockedSecureConnectScheme();
}
