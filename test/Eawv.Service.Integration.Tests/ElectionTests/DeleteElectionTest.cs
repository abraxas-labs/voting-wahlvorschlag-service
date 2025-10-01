// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Xunit;

namespace Eawv.Service.Integration.Tests.ElectionTests;

public class DeleteElectionTest : BaseRestTest
{
    private const string Url = "api/elections/";

    public DeleteElectionTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ElectionMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        await AssertStatus(
            () => ElectionAdminClient.DeleteAsync(Url + ElectionMockData.MajorzElection.Id),
            HttpStatusCode.OK);
    }

    [Fact]
    public async Task TestDifferentTenantShouldNotWork()
    {
        await AssertStatus(
            () => ElectionAdminClient.DeleteAsync(Url + ElectionMockData.GossauElection.Id),
            HttpStatusCode.NotFound);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.DeleteAsync(Url + ElectionMockData.MajorzElection.Id);
    }
}
