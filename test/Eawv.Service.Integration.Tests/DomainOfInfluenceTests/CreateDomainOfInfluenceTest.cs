// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Models;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.DomainOfInfluenceTests;

public class CreateDomainOfInfluenceTest : BaseRestTest
{
    private const string Url = "api/domainofinfluences";

    public CreateDomainOfInfluenceTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var doi = await GetSuccessfulResponse<DomainOfInfluenceModel>(() => ElectionAdminClient.PostAsJsonAsync(Url, NewValidRequest()));
        doi.MatchSnapshot(x => x.Id);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PostAsJsonAsync(Url, NewValidRequest());
    }

    private ModifyDomainOfInfluenceModel NewValidRequest()
    {
        return new ModifyDomainOfInfluenceModel
        {
            Name = "New doi",
            DomainOfInfluenceType = DomainOfInfluenceType.MU,
            OfficialId = "official ID",
            ShortName = "short name",
        };
    }
}
