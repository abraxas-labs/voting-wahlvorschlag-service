// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
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

namespace Eawv.Service.Integration.Tests.CandidateTests;

public class UpdateAllCandidatesTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/lists/{ListMockData.ProporzFdpList.Id}/candidates/batch";
    private static readonly string UrlArchivedElection = $"api/elections/{ElectionMockData.ArchivedElection.Id}/lists/{ListMockData.ArchivedElectionFdpList.Id}/candidates/batch";

    public UpdateAllCandidatesTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ElectionMockData.Seed(RunScoped);
        await ListMockData.Seed(RunScoped);
        await CandidateMockData.Seed(RunScoped);
        await DomainOfInfluenceMockData.Seed(RunScoped);
        await DomainOfInfluenceElectionMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var candidates = await GetSuccessfulResponse<List<CandidateModel>>(() => ElectionAdminClient.PutAsJsonAsync(Url, NewValidRequest()));
        candidates.MatchSnapshot(x => x.Id);
    }

    [Fact]
    public async Task TestAsUser()
    {
        var candidates = await GetSuccessfulResponse<List<CandidateModel>>(() => UserClient.PutAsJsonAsync(Url, NewValidRequest()));
        candidates.MatchSnapshot(x => x.Id);
    }

    [Fact]
    public async Task ShouldReturnBadRequestWhenOrderIndexesOutOfRange()
    {
        await AssertStatus(
            () => UserClient.PutAsJsonAsync(Url, NewValidRequest(c => c[0].Index = -1)), HttpStatusCode.BadRequest);
        await AssertStatus(
            () => UserClient.PutAsJsonAsync(Url, NewValidRequest(c => c[0].OrderIndex = 0)), HttpStatusCode.BadRequest);
        await AssertStatus(
            () => UserClient.PutAsJsonAsync(Url, NewValidRequest(c => c[0].CloneOrderIndex = 0)), HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task TestDifferentTenantShouldNotWork()
    {
        await AssertStatus(
            () => UserClient.PutAsJsonAsync(
                $"api/elections/{ElectionMockData.GossauElection.Id}/lists/{ListMockData.GossauList.Id}/candidates/batch",
                NewValidRequest()),
            HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Ensures that updating candidates on an archived election returns BadRequest.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TestWithArchivedElectionShouldThrow()
    {
        await AssertStatus(
            () => ElectionAdminClient.PutAsJsonAsync(UrlArchivedElection, NewValidRequest()),
            HttpStatusCode.BadRequest);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.User;
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PutAsJsonAsync(Url, NewValidRequest());
    }

    private List<ModifyCandidateModel> NewValidRequest(Action<List<ModifyCandidateModel>> customizer = null)
    {
        var candidates = new List<ModifyCandidateModel>
        {
            new()
            {
                Id = CandidateMockData.ProporzFdpListCandidate.Id,
                FirstName = "Max updated",
                FamilyName = "Muster updated",
                BallotFirstName = "Max updated",
                BallotFamilyName = "Muster updated",
                Locality = "St. Gallen updated",
                OccupationalTitle = "Bäcker updated",
                BallotLocality = "St. Gallen updated",
                BallotOccupationalTitle = "Bäcker updated",
                Index = 1,
                DateOfBirth = new DateTime(1956, 2, 3, 0, 0, 0, DateTimeKind.Utc),
                OrderIndex = 1,
                ZipCode = "9000",
                Sex = SexType.Male,
                Street = "Teststreet updated",
            },
            new()
            {
                FirstName = "New",
                FamilyName = "Newman",
                BallotFirstName = "New",
                BallotFamilyName = "Newman",
                Locality = "St. Gallen",
                OccupationalTitle = "CIO",
                BallotLocality = "St. Gallen",
                BallotOccupationalTitle = "CIO",
                Index = 2,
                DateOfBirth = new DateTime(1982, 6, 13, 0, 0, 0, DateTimeKind.Utc),
                OrderIndex = 2,
                ZipCode = "9000",
                Sex = SexType.Male,
                Street = "Downtown",
            },
        };

        customizer?.Invoke(candidates);
        return candidates;
    }
}
