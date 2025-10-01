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

namespace Eawv.Service.Integration.Tests.CandidateTests;

public class GetCandidatesTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/lists/{ListMockData.ProporzFdpList.Id}/candidates";

    public GetCandidatesTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ElectionMockData.Seed(RunScoped);
        await ListMockData.Seed(RunScoped);
        await CandidateMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var candidates = await GetSuccessfulResponse<List<CandidateModel>>(() => ElectionAdminClient.GetAsync(Url));
        candidates.MatchSnapshot();
    }

    [Fact]
    public async Task TestAsUser()
    {
        var candidates = await GetSuccessfulResponse<List<CandidateModel>>(() => UserClient.GetAsync(Url));
        candidates.MatchSnapshot();
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.User;
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.GetAsync(Url);
    }
}
