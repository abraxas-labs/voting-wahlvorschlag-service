// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Xunit;

namespace Eawv.Service.Integration.Tests.DomainOfInfluenceTests;

public class DeleteDomainOfInfluenceTest : BaseRestTest
{
    private const string Url = "api/domainofinfluences/";

    public DeleteDomainOfInfluenceTest(TestApplicationFactory factory)
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
        await AssertStatus(
            () => ElectionAdminClient.DeleteAsync(Url + DomainOfInfluenceMockData.StGallen.Id),
            HttpStatusCode.OK);
    }

    [Fact]
    public async Task TestDifferentTenantShouldBeForbidden()
    {
        await AssertStatus(
            () => ElectionAdminClient.DeleteAsync(Url + DomainOfInfluenceMockData.Gossau.Id),
            HttpStatusCode.Forbidden);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.DeleteAsync(Url + DomainOfInfluenceMockData.StGallen.Id);
    }
}
