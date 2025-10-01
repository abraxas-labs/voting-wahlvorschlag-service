// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Eawv.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voting.Lib.Common;

namespace Eawv.Service.Controllers;

[Route("api/elections/{electionId:Guid}/lists/unions")]
[ApiController]
[Authorize(Roles = Role.Wahlverwalter)]
public class ListUnionController
{
    private readonly IMapper _mapper;
    private readonly ElectionRepository _electionRepository;
    private readonly ListRepository _listRepository;
    private readonly ListUnionRepository _listUnionRepository;
    private readonly IClock _clock;

    public ListUnionController(
        IMapper mapper,
        ElectionRepository electionRepository,
        ListRepository listRepository,
        ListUnionRepository listUnionRepository,
        IClock clock)
    {
        _mapper = mapper;
        _electionRepository = electionRepository;
        _listRepository = listRepository;
        _listUnionRepository = listUnionRepository;
        _clock = clock;
    }

    [HttpPut("sub/{rootListId:Guid}")]
    public async Task<ListUnionModel> AddOrUpdateListSubUnion(
        Guid electionId,
        Guid rootListId,
        [FromBody, MaxLength(20)] List<Guid> listIds)
    {
        return await AddOrUpdateListUnion(electionId, listIds, rootListId);
    }

    [HttpPut]
    public async Task<ListUnionModel> AddOrUpdateListUnion(Guid electionId, [FromBody, MaxLength(20)] List<Guid> listIds)
    {
        return await AddOrUpdateListUnion(electionId, listIds, null);
    }

    [HttpDelete("sub/{listId:Guid}")]
    public async Task DeleteFromSubUnion(Guid electionId, Guid listId)
    {
        await DeleteFromUnion(electionId, listId, true);
    }

    [HttpDelete("{listId:Guid}")]
    public async Task DeleteFromUnion(Guid electionId, Guid listId)
    {
        await DeleteFromUnion(electionId, listId, false);
    }

    private async Task DeleteFromUnion(Guid electionId, Guid listId, bool subUnion)
    {
        var list = await _listRepository.Get(electionId, listId);

        if (list.Election.IsArchived(_clock))
        {
            throw new BadRequestException("An archived election can't be modified.");
        }

        var listUnion = (subUnion ? list.ListSubUnion : list.ListUnion) ?? throw new EntityNotFoundException(list.Id);

        if (listUnion.Lists.Count <= 2 || listUnion.RootListId == listId)
        {
            await _listUnionRepository.Delete(listUnion.Id);
            return;
        }

        listUnion.Lists.Remove(list);
        await _listUnionRepository.Update(listUnion);
    }

    /// <summary>
    /// Creates a new ListUnion, or updates an existing one with the new lists.
    /// </summary>
    /// <param name="electionId">The election of the lists.</param>
    /// <param name="listIds">the lists of the existing union with lists to be added.</param>
    /// <param name="rootListId">if it's a subunion, this is the rootlist.</param>
    /// <returns>The created or updated union list.</returns>
    /// <exception cref="BadRequestException">Thrown if the count of the passed listIds is less than two. At least two are required for a union.</exception>
    /// <exception cref="EntityNotFoundException">Thrown if the count of the lists from the repository is not equals the passed listIds.</exception>
    private async Task<ListUnionModel> AddOrUpdateListUnion(Guid electionId, List<Guid> listIds, Guid? rootListId)
    {
        if (rootListId.HasValue && !listIds.Contains(rootListId.Value))
        {
            listIds.Add(rootListId.Value);
        }

        if (listIds.Count < 2)
        {
            throw new BadRequestException("At least 2 lists are needed for a list union");
        }

        // checks access rights on this election
        var election = await _electionRepository.Get(electionId);

        if (election.IsArchived(_clock))
        {
            throw new BadRequestException("An archived election can't be modified.");
        }

        var lists = await _listRepository.GetListsForUnions(electionId, listIds);

        if (lists.Count != listIds.Count)
        {
            throw new EntityNotFoundException(string.Join(", ", listIds));
        }

        var listUnions = lists.Select(l => rootListId.HasValue ? l.ListSubUnion : l.ListUnion)
            .Where(lu => lu != null)
            .Distinct()
            .ToList();

        ListUnion union;
        if (listUnions.Count == 0)
        {
            union = await _listUnionRepository.Create(new ListUnion(rootListId, lists));
        }
        else
        {
            union = await Merge(listUnions, lists, rootListId);
        }

        return _mapper.Map<ListUnionModel>(union);
    }

    private async Task<ListUnion> Merge(List<ListUnion> listUnions, IList<List> lists, Guid? rootListId)
    {
        var union = listUnions[0];
        union.RootListId = rootListId;
        union.Lists = listUnions.SelectMany(u => u.Lists)
            .Distinct()
            .ToList();

        var existingIds = union.Lists.Select(l => l.Id).ToList();
        foreach (var list in lists)
        {
            if (existingIds.Contains(list.Id))
            {
                continue;
            }

            union.Lists.Add(list);
        }

        return await _listUnionRepository.KeepFirst(listUnions);
    }
}
