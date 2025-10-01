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

public class CreateDomainOfInfluenceElectionTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/domainofinfluences";

    public CreateDomainOfInfluenceElectionTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await DomainOfInfluenceMockData.Seed(RunScoped);
        await ElectionMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var doi = await GetSuccessfulResponse<DomainOfInfluenceElectionModel>(() => ElectionAdminClient.PostAsJsonAsync(Url, NewValidRequest()));
        doi.MatchSnapshot();
    }

    [Fact]
    public async Task TestWithDifferentTenantDoiShouldNotWork()
    {
        await AssertStatus(
            () => ElectionAdminClient.PostAsJsonAsync(Url, new CreateDomainOfInfluenceElectionModel
            {
                Id = DomainOfInfluenceMockData.Gossau.Id,
                NumberOfMandates = 3,
            }),
            HttpStatusCode.Forbidden);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PostAsJsonAsync(Url, NewValidRequest());
    }

    private CreateDomainOfInfluenceElectionModel NewValidRequest()
    {
        return new CreateDomainOfInfluenceElectionModel
        {
            Id = DomainOfInfluenceMockData.StGallen.Id,
            NumberOfMandates = 3,
        };
    }
}
