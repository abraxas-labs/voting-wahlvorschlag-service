// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Xunit;

namespace Eawv.Service.Integration.Tests.DomainOfInfluenceElectionTests;

public class DeleteDomainOfInfluenceElectionTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/domainofinfluences/";
    private static readonly string UrlArchivedElection = $"api/elections/{ElectionMockData.ArchivedElection.Id}/domainofinfluences/";

    public DeleteDomainOfInfluenceElectionTest(TestApplicationFactory factory)
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
        await AssertStatus(
            () => ElectionAdminClient.DeleteAsync(Url + DomainOfInfluenceMockData.StGallen.Id),
            HttpStatusCode.OK);
    }

    [Fact]
    public async Task TestWithDifferentTenantDoiShouldNotWork()
    {
        await AssertStatus(
            () => ElectionAdminClient.DeleteAsync($"api/elections/{ElectionMockData.GossauElection.Id}/domainofinfluences/{DomainOfInfluenceMockData.Gossau.Id}"),
            HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Ensures that deleting a domain of influence election on an archived election returns BadRequest.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TestWithArchivedElectionShouldThrow()
    {
        await AssertStatus(
            () => ElectionAdminClient.DeleteAsync(UrlArchivedElection + DomainOfInfluenceMockData.StGallen.Id),
            HttpStatusCode.BadRequest);
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
