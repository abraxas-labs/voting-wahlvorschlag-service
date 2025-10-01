// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Eawv.Service.ModelBinders;
using Eawv.Service.Models;
using Eawv.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voting.Lib.Common;

namespace Eawv.Service.Controllers;

[Route("api/elections/{electionId:Guid}/lists")]
[ApiController]
[Authorize(Roles = Role.All)]
public class ListController
{
    private readonly EventNotificationService _eventNotificationService;
    private readonly ElectionRepository _electionRepository;
    private readonly ListRepository _listRepository;
    private readonly IListService _listService;
    private readonly IMapper _mapper;
    private readonly AuthService _authService;
    private readonly IUserService _userService;
    private readonly ITenantService _tenantService;
    private readonly IClock _clock;

    public ListController(
        EventNotificationService eventNotificationService,
        ElectionRepository electionRepository,
        ListRepository listRepository,
        IListService listService,
        IMapper mapper,
        AuthService authService,
        IUserService userService,
        ITenantService tenantService,
        IClock clock)
    {
        _eventNotificationService = eventNotificationService;
        _electionRepository = electionRepository;
        _listRepository = listRepository;
        _listService = listService;
        _mapper = mapper;
        _authService = authService;
        _userService = userService;
        _tenantService = tenantService;
        _clock = clock;
    }

    [HttpGet]
    public async Task<IEnumerable<ListModel>> GetLists(Guid electionId)
    {
        return await _listService.GetLists(electionId);
    }

    [HttpGet("{id:Guid}")]
    public async Task<ListModel> GetList(Guid electionId, Guid id)
    {
        return _mapper.Map<ListModel>(await _listRepository.Get(electionId, id));
    }

    [HttpPost]
    public async Task<ListModel> CreateList(Guid electionId, [FromBody] ModifyListModel listModel)
    {
        var election = await _electionRepository.GetSimpleElection(electionId);

        if (election.IsArchived(_clock))
        {
            throw new BadRequestException("An archived election can't be modified.");
        }

        var list = _mapper.Map<List>(listModel);
        list.ElectionId = electionId;

        if (_authService.IsWahlverwalter)
        {
            _authService.AssertAdminOnElection(election);
        }
        else
        {
            // if its not an admin, certain properties cannot be set
            list.Indenture = null;
            list.SubmitDate = null;
            list.ResponsiblePartyTenantId = _authService.GetTenantId();
            list.Validated = false;
        }

        await _tenantService.AssertChildParent(list.ResponsiblePartyTenantId, election.TenantId);
        await EnsureValidList(election, list);
        await EnsureCorrectUsers(list);

        list.State = ListState.Draft;

        return _mapper.Map<ListModel>(await _listRepository.Create(list));
    }

    [HttpPut("{id:Guid}")]
    public async Task<ListModel> UpdateList(Guid electionId, Guid id, [FromBody] ModifyListModel listModel)
    {
        var existing = await _listRepository.Get(electionId, id);

        var election = await _electionRepository.GetSimpleElection(electionId);

        if (election.IsArchived(_clock))
        {
            throw new BadRequestException("An archived election can't be modified.");
        }

        var list = _mapper.Map<List>(listModel);
        list.Id = id;
        list.ElectionId = electionId;
        list.State = existing.State;

        _authService.AssertListWriteAccess(existing);

        // only admins can edit certain properties
        if (!_authService.IsWahlverwalter)
        {
            list.Indenture = existing.Indenture;
            list.ResponsiblePartyTenantId = existing.ResponsiblePartyTenantId;
            list.Locked = false;
            list.Validated = existing.Validated;
            list.SubmitDate = existing.SubmitDate;
        }

        if (existing.ResponsiblePartyTenantId != list.ResponsiblePartyTenantId)
        {
            await _tenantService.AssertChildParent(list.ResponsiblePartyTenantId, election.TenantId);
        }

        await EnsureValidList(election, list);
        await EnsureCorrectUsers(list, existing);

        list.Version = existing.Version + 1;

        var savedList = await _listRepository.Update(list);
        return _mapper.Map<ListModel>(savedList);
    }

    /// <summary>
    /// Currently only supports patching of the list state.
    /// </summary>
    /// <param name="electionId">The Guid of the election.</param>
    /// <param name="id">The Guid of the list.</param>
    /// <param name="theme">The theme for the E-Mail Notification URL.</param>
    /// <param name="listModel">The model of the list to patch.</param>
    /// <returns>The updated list model.</returns>
    [HttpPatch("{id:Guid}")]
    public async Task<ListModel> UpdatePartialList(
        Guid electionId,
        Guid id,
        [ModelBinder(BinderType = typeof(CaseInsensitiveEnumBinder<Theme>))] Theme theme,
        [FromBody] PatchListModel listModel)
    {
        var list = _mapper.Map<List>(listModel);
        list.Id = id;
        list.ElectionId = electionId;

        var existing = await _listRepository.Get(electionId, id);
        var election = await _electionRepository.GetSimpleElection(electionId);

        if (election.IsArchived(_clock))
        {
            throw new BadRequestException("An archived election can't be modified.");
        }

        if (!IsNewStateValid(existing.State, list.State))
        {
            throw new InvalidStateException(existing.State, list.State);
        }

        _authService.AssertListWriteAccess(existing);

        var oldState = existing.State;
        existing.State = listModel.State;

        var savedList = await _listRepository.Update(existing);

        if (savedList.State != oldState)
        {
            await _eventNotificationService.ListStateChanged(election, savedList, theme);
        }

        return _mapper.Map<ListModel>(savedList);
    }

    [HttpDelete("{id:Guid}")]
    public async Task DeleteList(Guid electionId, Guid id)
    {
        var existing = await _listRepository.Get(electionId, id);

        if (existing.Election.IsArchived(_clock))
        {
            throw new BadRequestException("An archived election can't be modified.");
        }

        _authService.AssertListWriteAccess(existing);
        await _listRepository.Delete(id);
    }

    private bool IsNewStateValid(ListState oldState, ListState newState)
    {
        if (oldState == ListState.Archived)
        {
            return false;
        }

        if (oldState == newState)
        {
            return true;
        }

        if (oldState != ListState.Draft && !_authService.IsWahlverwalter)
        {
            return false;
        }

        return oldState switch
        {
            ListState.Draft
                => newState == ListState.Submitted,
            ListState.Submitted
                => newState == ListState.FormallySubmitted || newState == ListState.Valid,
            ListState.FormallySubmitted
                => newState == ListState.Submitted || newState == ListState.Valid,
            ListState.Valid
                => newState == ListState.Submitted || newState == ListState.FormallySubmitted,
            _ => false,
        };
    }

    private async Task EnsureValidList(Election election, List updatedList)
    {
        if (election.ElectionType != ElectionType.Proporz || updatedList.Indenture == null)
        {
            return;
        }

        var lists = await _listRepository.GetListsForElection(election.Id);
        var otherListIndentures = lists
            .Where(l => l.Id != updatedList.Id)
            .Select(l => l.Indenture)
            .ToHashSet();

        if (otherListIndentures.Contains(updatedList.Indenture))
        {
            throw new DuplicateListIndentureException(election.Id, updatedList.Indenture);
        }
    }

    private async Task EnsureCorrectUsers(List updatedList, List listBeforeSave = null)
    {
        if (updatedList.DeputyUsers == null)
        {
            updatedList.DeputyUsers = [];
        }
        else
        {
            updatedList.DeputyUsers.RemoveAll(o => o == null);
        }

        if (updatedList.MemberUsers == null)
        {
            updatedList.MemberUsers = [];
        }
        else
        {
            updatedList.MemberUsers.RemoveAll(o => o == null);
        }

        // check if new set users exists and has access
        var newUserIds = updatedList.DeputyUsers
            .Concat(updatedList.MemberUsers)
            .ToHashSet();

        if (listBeforeSave != null)
        {
            newUserIds.ExceptWith(listBeforeSave.DeputyUsers);
            newUserIds.ExceptWith(listBeforeSave.MemberUsers);
        }

        if (listBeforeSave == null || listBeforeSave.Representative != updatedList.Representative)
        {
            newUserIds.Add(updatedList.Representative);
        }

        var tenantUserIds = (await _userService.GetUsersForTenant(updatedList.ResponsiblePartyTenantId))
            .Select(u => u.User.Loginid)
            .ToHashSet();

        if (!newUserIds.IsSubsetOf(tenantUserIds))
        {
            throw new ForbiddenException("Not all provided users are from the list tenant.");
        }
    }
}
