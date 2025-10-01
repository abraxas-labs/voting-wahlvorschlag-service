// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.DataAccess;
using Eawv.Service.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Testing.Mocks;

namespace Eawv.Service.Integration.Tests.MockedData;

public static class ListCommentMockData
{
    public static ListComment ProporzFdpListComment => new()
    {
        Id = new Guid("3d5e17d6-7809-4e94-85b3-de54b5499ca8"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.TestUser.Id,
        Content = "test comment",
        ListId = ListMockData.ProporzFdpList.Id,
    };

    public static ListComment ProporzSpListComment => new()
    {
        Id = new Guid("0e2117dd-e83f-4217-a6a8-84015ec6a5d0"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.SpUser.Id,
        Content = "TEST",
        ListId = ListMockData.ProporzSpList.Id,
    };

    public static ListComment GossauListComment => new()
    {
        Id = new Guid("54d805ed-5596-43b9-a70d-d0574570f699"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.GossauUser.Id,
        Content = "test comment on gossau list",
        ListId = ListMockData.GossauList.Id,
    };

    public static IEnumerable<ListComment> All
    {
        get
        {
            yield return ProporzFdpListComment;
            yield return ProporzSpListComment;
            yield return GossauListComment;
        }
    }

    public static Task Seed(Func<Func<IServiceProvider, Task>, Task> runScoped)
    {
        var all = All.ToList();
        return runScoped(async sp =>
        {
            var db = sp.GetRequiredService<EawvContext>();
            db.ListComments.AddRange(all);
            await db.SaveChangesAsync();
        });
    }
}
