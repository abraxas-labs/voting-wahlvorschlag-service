// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.DomainOfInfluenceTests;

public class UpdateDomainOfInfluenceTest : BaseRestTest
{
    private const string Url = "api/domainofinfluences/";

    public UpdateDomainOfInfluenceTest(TestApplicationFactory factory)
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
        var doi = await GetSuccessfulResponse<DomainOfInfluenceModel>(
            () => ElectionAdminClient.PutAsJsonAsync(Url + DomainOfInfluenceMockData.StGallen.Id, NewValidRequest()));
        doi.MatchSnapshot();
    }

    [Fact]
    public async Task TestDifferentTenantShouldBeForbidden()
    {
        await AssertStatus(
            () => ElectionAdminClient.PutAsJsonAsync(Url + DomainOfInfluenceMockData.Gossau.Id, NewValidRequest()),
            HttpStatusCode.Forbidden);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PutAsJsonAsync(Url + DomainOfInfluenceMockData.StGallen.Id, NewValidRequest());
    }

    private ModifyDomainOfInfluenceModel NewValidRequest()
    {
        return new ModifyDomainOfInfluenceModel
        {
            Name = "Update doi",
            DomainOfInfluenceType = DomainOfInfluenceType.MU,
            OfficialId = "updated official ID",
            ShortName = "Updated short name",
        };
    }
}
