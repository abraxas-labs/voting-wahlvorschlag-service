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

[Route("api/elections/{electionId:Guid}/lists/{listId:Guid}/comments")]
[ApiController]
[Authorize(Roles = Role.All)]
public class ListCommentController
{
    private readonly ListCommentRepository _commentRepository;
    private readonly IMapper _mapper;
    private readonly AuthService _authService;
    private readonly EventNotificationService _eventNotificationService;
    private readonly ListRepository _listRepository;
    private readonly IUserService _userService;
    private readonly IClock _clock;

    public ListCommentController(
        ListCommentRepository commentRepository,
        IMapper mapper,
        AuthService authService,
        EventNotificationService eventNotificationService,
        ListRepository listRepository,
        IUserService userService,
        IClock clock)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
        _authService = authService;
        _eventNotificationService = eventNotificationService;
        _listRepository = listRepository;
        _userService = userService;
        _clock = clock;
    }

    [HttpGet]
    public async Task<IEnumerable<ListCommentModel>> GetComments(Guid listId)
    {
        var comments = await _commentRepository.GetCommentsForList(listId);
        var mapped = _mapper.Map<List<ListCommentModel>>(comments);
        await AddUserInfoToComments(mapped);
        return mapped;
    }

    [HttpPost]
    public async Task<ListCommentModel> CreateComment(
        Guid electionId,
        Guid listId,
        [ModelBinder(BinderType = typeof(CaseInsensitiveEnumBinder<Theme>))] Theme theme,
        [FromBody] ModifyListCommentModel commentModel)
    {
        var comment = _mapper.Map<ListComment>(commentModel);
        comment.ListId = listId;

        var list = await _listRepository.Get(electionId, listId);

        if (list.Election.IsArchived(_clock))
        {
            throw new BadRequestException("An archived election can't be modified.");
        }

        // Only read permissions on the list are needed for creating comments
        // Otherwise, users couldn't comment on locked lists or lists that aren't in the Draft state
        _authService.AssertListReadPermissions(list);

        var createdComment = await _commentRepository.Create(comment);
        await _eventNotificationService.ListNewComment(list.Election, list, theme);

        return _mapper.Map<ListCommentModel>(createdComment);
    }

    [HttpPut("{id:Guid}")]
    public async Task<ListCommentModel> UpdateComment(
        Guid electionId,
        Guid listId,
        Guid id,
        [FromBody] ModifyListCommentModel commentModel)
    {
        var comment = _mapper.Map<ListComment>(commentModel);
        comment.ListId = listId;
        comment.Id = id;

        var existingComment = await _commentRepository.Get(id);

        if (existingComment.List.Election.IsArchived(_clock))
        {
            throw new BadRequestException("An archived election can't be modified.");
        }

        if (existingComment.CreatedBy != _authService.GetUserId())
        {
            throw new ForbiddenException(id);
        }

        await AssertWriteAccess(listId, electionId);
        return _mapper.Map<ListCommentModel>(await _commentRepository.Update(comment));
    }

    [HttpDelete("{id:Guid}")]
    public async Task DeleteComment(Guid electionId, Guid listId, Guid id)
    {
        var existingComment = await _commentRepository.Get(id);

        if (existingComment.List.Election.IsArchived(_clock))
        {
            throw new BadRequestException("An archived election can't be modified.");
        }

        if (existingComment.CreatedBy != _authService.GetUserId())
        {
            throw new ForbiddenException(id);
        }

        await AssertWriteAccess(listId, electionId);
        await _commentRepository.Delete(listId, id);
    }

    private async Task AssertWriteAccess(Guid listId, Guid electionId)
    {
        var list = await _listRepository.Get(electionId, listId);

        if (list.State >= ListState.FormallySubmitted)
        {
            throw new ForbiddenException(list.Id);
        }

        // Only read permissions on the list are needed for creating comments
        // Otherwise, users couldn't comment on locked lists or lists that aren't in the Draft state
        _authService.AssertListReadPermissions(list);
    }

    private async Task AddUserInfoToComments(List<ListCommentModel> comments)
    {
        var commentsByUserId = comments
            .GroupBy(c => c.CreatedBy)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var (userId, userComments) in commentsByUserId)
        {
            try
            {
                var user = await _userService.Get(userId);
                foreach (var comment in userComments)
                {
                    comment.CreatorFirstName = user.Firstname;
                    comment.CreatorLastName = user.Lastname;
                }
            }
            catch
            {
                // It does not matter too much if we cannot fetch a user. Maybe it was deleted.
            }
        }
    }
}
