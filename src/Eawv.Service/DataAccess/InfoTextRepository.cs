// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Eawv.Service.Extensions;
using Eawv.Service.Services;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Common;

namespace Eawv.Service.DataAccess;

public class InfoTextRepository : BaseRepository<InfoText>
{
    private readonly ITenantService _tenantService;

    public InfoTextRepository(EawvContext context, AuthService authService, ITenantService tenantService, IClock clock)
        : base(context, authService, clock)
    {
        _tenantService = tenantService;
    }

    /// <summary>
    /// Gets the best matching info text list containing one representative for each key.
    /// Note: GroupBy does not work anymore within ef core for net5.0, because of LINQ limitations. It used to work for netcoreapp2.2.
    /// Therefore the grouping is done server side.
    /// </summary>
    /// <param name="electionId">The electionId to look up the info texts for.</param>
    /// <param name="tenantId">The tennant id to look up the info texts for.</param>
    /// <returns>A collection of <see cref="InfoText"/>.</returns>
    public async Task<IEnumerable<InfoText>> GetBestMatching(Guid? electionId, string tenantId)
    {
        var orderedInfoTexts = await OrderedInfoTexts(electionId, tenantId).ToListAsync();

        return orderedInfoTexts.GroupBy(it => it.Key, (_, texts) => texts.First());
    }

    public async Task<InfoText> GetBestMatching(string key, Guid? electionId, string tenantId)
    {
        return await OrderedInfoTexts(electionId, tenantId)
                   .Where(it => it.Key == key)
                   .FirstOrDefaultAsync() ?? throw new EntityNotFoundException(key);
    }

    public override async Task<InfoText> Get(Guid id)
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        var it = await base.Get(id);
        if (it.TenantId != tenantId)
        {
            throw new ForbiddenException(id);
        }

        return it;
    }

    public async Task<IEnumerable<InfoText>> CreateOrUpdate(IEnumerable<InfoText> infoTexts)
    {
        var saved = new List<InfoText>();

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        foreach (var it in infoTexts)
        {
            saved.Add(await CreateOrUpdate(it));
        }

        await Save(true);
        var ids = saved.Select(it => it.Id).Peek(Detach).ToArray();
        var updatedInfotexts = await Context.InfoTexts.Where(it => ids.Contains(it.Id)).ToListAsync();
        transaction.Complete();
        return updatedInfotexts;
    }

    public async Task<InfoText> CreateOrUpdate(InfoText infoText)
    {
        var existingInfoText = await Context.InfoTexts
            .FirstOrDefaultAsync(it =>
                it.Key == infoText.Key && it.ElectionId == infoText.ElectionId && it.TenantId == infoText.TenantId);

        if (existingInfoText == null)
        {
            return await Create(infoText);
        }

        infoText.Id = existingInfoText.Id;
        return await Update(infoText);
    }

    private IOrderedQueryable<InfoText> OrderedInfoTexts(Guid? electionId = null, string tenantId = null)
    {
        return Context.InfoTexts
                .Where(it => it.ElectionId == electionId && it.TenantId == tenantId)
            .OrderByDescending(it => it.ElectionId == electionId);
    }
}
