// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess;
using Eawv.Service.Integration.Tests.MockedData;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Testing;

namespace Eawv.Service.Integration.Tests;

public abstract class BaseRestTest : RestAuthorizationBaseTest<TestApplicationFactory, TestStartup>
{
    private static readonly JsonSerializerOptions JsonSettings = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly Lazy<HttpClient> _electionAdminClient;
    private readonly Lazy<HttpClient> _userClient;

    protected BaseRestTest(TestApplicationFactory factory)
        : base(factory)
    {
        _electionAdminClient = new Lazy<HttpClient>(() => CreateHttpClient(Role.Wahlverwalter));
        _userClient = new Lazy<HttpClient>(() => CreateHttpClient(Role.User));
    }

    protected HttpClient ElectionAdminClient => _electionAdminClient.Value;

    protected HttpClient UserClient => _userClient.Value;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ResetDb();
    }

    protected abstract IEnumerable<string> AuthorizedRoles();

    protected override IEnumerable<string> UnauthorizedRoles()
    {
        return Role
            .AllRoles()
            .Append(NoRole)
            .Except(AuthorizedRoles());
    }

    protected override HttpClient CreateHttpClient(params string[] roles)
    {
        if (roles is [Role.User])
        {
            // When requesting a user role, use a different tenant.
            // In practice, users on the parent tenant are always Wahlverwalter,
            // on child tenants they are Users.
            // Thus we cannot use the parent tenant for users, as that does not make sense in the real world.
            return CreateHttpClient(true, TenantMockData.FdpStGallen.Id, UserMockData.TestUser.Id, roles);
        }

        return CreateHttpClient(true, TenantMockData.StGallen.Id, UserMockData.TestUser.Id, roles);
    }

    protected async Task<T> GetSuccessfulResponse<T>(Func<Task<HttpResponseMessage>> httpCall)
    {
        var response = await AssertStatus(httpCall, HttpStatusCode.OK);
        return await response.Content.ReadFromJsonAsync<T>(JsonSettings);
    }

    protected Task RunOnDb(Func<EawvContext, Task> action)
        => RunScoped(action);

    protected Task<TResult> RunOnDb<TResult>(Func<EawvContext, Task<TResult>> action)
        => RunScoped(action);

    private async Task ResetDb()
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EawvContext>();
        await DatabaseUtil.Truncate(db);
    }
}
