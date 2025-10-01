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

[Route("api/elections/{electionId:Guid}/lists/{listId:Guid}/candidates")]
[ApiController]
[Authorize(Roles = Role.All)]
public class CandidateController
{
    private readonly CandidateRepository _candidateRepository;
    private readonly ElectionRepository _electionRepository;
    private readonly IMapper _mapper;
    private readonly AuthService _authService;
    private readonly ListRepository _listRepository;
    private readonly IClock _clock;

    public CandidateController(
        CandidateRepository candidateRepository,
        ElectionRepository electionRepository,
        IMapper mapper,
        AuthService authService,
        ListRepository listRepository,
        IClock clock)
    {
        _candidateRepository = candidateRepository;
        _electionRepository = electionRepository;
        _mapper = mapper;
        _authService = authService;
        _listRepository = listRepository;
        _clock = clock;
    }

    [HttpGet]
    public async Task<IEnumerable<CandidateModel>> GetCandidates(Guid listId)
    {
        var candidates = await _candidateRepository.GetCandidatesForList(listId);
        return _mapper.Map<IEnumerable<CandidateModel>>(candidates);
    }

    /// <summary>
    /// Replaces all candidates with the ones in the request.
    /// </summary>
    /// <param name="electionId">The Guid of the election, containing the list to update.</param>
    /// <param name="listId">The Guid of the list to update.</param>
    /// <param name="candidateModels">A collection of candidates to update.</param>
    /// <returns>A collection of candidates.</returns>
    [HttpPut("batch")]
    public async Task<IEnumerable<CandidateModel>> UpdateAllCandidates(
        Guid electionId,
        Guid listId,
        [FromBody, MinLength(1), MaxLength(200)] IList<ModifyCandidateModel> candidateModels)
    {
        var candidates = _mapper.Map<IList<Candidate>>(candidateModels);
        await AssertWriteAccess(listId, electionId);

        var totalMandates = await _electionRepository.GetTotalNumberOfMandates(electionId);
        var candidateCount = candidates.Count + candidates.Count(c => c.Cloned);
        if (candidateCount > totalMandates)
        {
            throw new TooManyCandidatesException(candidateCount, totalMandates);
        }

        for (var i = 0; i < candidates.Count; i++)
        {
            if (candidates[i].Index == 0)
            {
                candidates[i].Index = i + 1;
            }

            candidates[i].ListId = listId;
        }

        // For users without the Wahlverwalter role, retain existing marked elements and disallow adding new ones.
        if (!_authService.IsWahlverwalter)
        {
            var existingCandidates = (await _candidateRepository.GetCandidatesForList(listId))
                .ToDictionary(candidate => candidate.Id);

            foreach (var candidate in candidates)
            {
                candidate.MarkedElements.Clear();

                if (existingCandidates.TryGetValue(candidate.Id, out var existing))
                {
                    candidate.MarkedElements = existing.MarkedElements
                        .Select(m => new MarkedElement { Field = m.Field })
                        .ToList();
                }
            }
        }

        await _candidateRepository.UpdateAll(listId, candidates);

        return _mapper.Map<IEnumerable<CandidateModel>>(await _candidateRepository.GetCandidatesForList(listId));
    }

    private async Task AssertWriteAccess(Guid listId, Guid electionId)
    {
        var list = await _listRepository.Get(electionId, listId);

        if (list.Election.IsArchived(_clock))
        {
            throw new BadRequestException("An archived election can't be modified.");
        }

        _authService.AssertListWriteAccess(list);
    }
}
