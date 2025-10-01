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

public static class InfoTextMockData
{
    public static InfoText BaseInfoText => new()
    {
        Id = new Guid("62f64bb4-0011-4bb1-a7ec-7d2b50cc194a"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.StGallenUser.Id,
        Key = "base",
        Value = "test base",
        Visible = true,
    };

    public static InfoText BaseInfoText2 => new()
    {
        Id = new Guid("bd10679d-7996-4be5-a942-efff8d515e1e"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.StGallenUser.Id,
        Key = "base2",
        Value = "test base2",
        Visible = true,
    };

    public static InfoText ElectionBaseInfoText => new()
    {
        Id = new Guid("1929ee8f-db48-4c07-8b9b-fbba0e6baf35"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.StGallenUser.Id,
        Key = "base",
        Value = "test base",
        Visible = true,
        ElectionId = ElectionMockData.ProporzElection.Id,
    };

    public static InfoText ElectionBaseInfoText2 => new()
    {
        Id = new Guid("7bccebf6-d8af-440f-b5fd-09ac33cd5bb3"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.StGallenUser.Id,
        Key = "base2",
        Value = "hidden",
        Visible = false,
        ElectionId = ElectionMockData.ProporzElection.Id,
    };

    public static InfoText GossauInfoText => new()
    {
        Id = new Guid("13359bb5-2777-4426-a3dd-e4ac89919bda"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.GossauUser.Id,
        Key = "base2",
        Value = "gossau",
        Visible = true,
        ElectionId = ElectionMockData.GossauElection.Id,
    };

    public static IEnumerable<InfoText> All
    {
        get
        {
            yield return BaseInfoText;
            yield return BaseInfoText2;
            yield return ElectionBaseInfoText;
            yield return ElectionBaseInfoText2;
            yield return GossauInfoText;
        }
    }

    public static Task Seed(Func<Func<IServiceProvider, Task>, Task> runScoped)
    {
        var all = All.ToList();
        return runScoped(async sp =>
        {
            var db = sp.GetRequiredService<EawvContext>();
            db.InfoTexts.AddRange(all);
            await db.SaveChangesAsync();
        });
    }
}
