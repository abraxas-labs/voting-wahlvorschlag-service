// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eawv.Service.DataAccess;
using Eawv.Service.Models;

namespace Eawv.Service.Services;

/// <inheritdoc cref="IListService"/>
public class ListService : IListService
{
    private readonly ListRepository _listRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListService"/> class.
    /// </summary>
    /// <param name="listRepository">The election list repository.</param>
    /// <param name="mapper">The mapper to map between database and api model.</param>
    /// <param name="userService">The user service to enrich user data.</param>
    public ListService(ListRepository listRepository, IMapper mapper, IUserService userService)
    {
        _listRepository = listRepository;
        _mapper = mapper;
        _userService = userService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ListModel>> GetLists(Guid electionId)
    {
        var lists = await _listRepository.GetListsForElection(electionId);
        var mappedLists = _mapper.Map<IEnumerable<ListModel>>(lists)
            .OrderBy(l => l.SortOrder).ToList();

        foreach (var list in mappedLists)
        {
            list.CreatedByName = await GetUserName(list.CreatedBy);
            list.ModifiedByName = await GetUserName(list.ModifiedBy);
        }

        return mappedLists;
    }

    private async Task<string> GetUserName(string loginId)
    {
        return (await _userService.Get(loginId))?.Username ?? string.Empty;
    }
}
