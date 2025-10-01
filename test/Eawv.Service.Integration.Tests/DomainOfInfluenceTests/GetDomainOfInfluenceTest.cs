// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using FluentAssertions;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.DomainOfInfluenceTests;

public class GetDomainOfInfluenceTest : BaseRestTest
{
    private const string Url = "api/domainofinfluences/";

    public GetDomainOfInfluenceTest(TestApplicationFactory factory)
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
        var doi = await GetSuccessfulResponse<DomainOfInfluenceModel>(() => ElectionAdminClient.GetAsync(Url + DomainOfInfluenceMockData.StGallen.Id));
        doi.MatchSnapshot();
    }

    [Fact]
    public async Task TestAsUser()
    {
        var doi = await GetSuccessfulResponse<DomainOfInfluenceModel>(() => UserClient.GetAsync(Url + DomainOfInfluenceMockData.StGallen.Id));
        doi.Id.Should().Be(DomainOfInfluenceMockData.StGallen.Id);
    }

    [Fact]
    public async Task TestDifferentTenant()
    {
        await AssertStatus(
            () => ElectionAdminClient.GetAsync(Url + DomainOfInfluenceMockData.Gossau.Id),
            HttpStatusCode.Forbidden);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
        yield return Role.User;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.GetAsync(Url + DomainOfInfluenceMockData.StGallen.Id);
    }
}
