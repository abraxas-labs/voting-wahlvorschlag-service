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

namespace Eawv.Service.Integration.Tests.DomainOfInfluenceTests;

public class GetDomainOfInfluencesTest : BaseRestTest
{
    private const string Url = "api/domainofinfluences";

    public GetDomainOfInfluencesTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await DomainOfInfluenceMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var dois = await GetSuccessfulResponse<List<DomainOfInfluenceModel>>(() => ElectionAdminClient.GetAsync(Url));
        dois.MatchSnapshot();
    }

    [Fact]
    public async Task TestAsUser()
    {
        var dois = await GetSuccessfulResponse<List<DomainOfInfluenceModel>>(() => UserClient.GetAsync(Url));
        dois.MatchSnapshot();
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
