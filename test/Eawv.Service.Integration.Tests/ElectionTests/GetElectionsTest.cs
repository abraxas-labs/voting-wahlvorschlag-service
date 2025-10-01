// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.ElectionTests;

public class GetElectionsTest : BaseRestTest
{
    private const string Url = "api/elections";

    public GetElectionsTest(TestApplicationFactory factory)
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
        var elections = await GetSuccessfulResponse<List<ElectionOverviewModel>>(() => ElectionAdminClient.GetAsync(Url));
        elections.MatchSnapshot();
    }

    [Fact]
    public async Task TestAsUser()
    {
        var elections = await GetSuccessfulResponse<List<ElectionOverviewModel>>(() => UserClient.GetAsync(Url));
        elections.MatchSnapshot();
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
        yield return Role.User;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.GetAsync(Url);
    }
}
