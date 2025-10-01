// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.DomainOfInfluenceElectionTests;

public class UpdateDomainOfInfluenceElectionTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/domainofinfluences/";

    public UpdateDomainOfInfluenceElectionTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await DomainOfInfluenceMockData.Seed(RunScoped);
        await ElectionMockData.Seed(RunScoped);
        await DomainOfInfluenceElectionMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var doi = await GetSuccessfulResponse<DomainOfInfluenceElectionModel>(
            () => ElectionAdminClient.PutAsJsonAsync(Url + DomainOfInfluenceMockData.StGallen.Id, NewValidRequest()));
        doi.MatchSnapshot();
    }

    [Fact]
    public async Task TestWithDifferentTenantDoiShouldNotWork()
    {
        await AssertStatus(
            () => ElectionAdminClient.PutAsJsonAsync(
                $"api/elections/{ElectionMockData.GossauElection.Id}/domainofinfluences/{DomainOfInfluenceMockData.Gossau.Id}",
                NewValidRequest()),
            HttpStatusCode.NotFound);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PutAsJsonAsync(Url + DomainOfInfluenceMockData.StGallen.Id, NewValidRequest());
    }

    private UpdateDomainOfInfluenceElectionModel NewValidRequest()
    {
        return new UpdateDomainOfInfluenceElectionModel
        {
            NumberOfMandates = 2,
        };
    }
}
