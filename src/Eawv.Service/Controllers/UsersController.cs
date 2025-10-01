// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Eawv.Service.Authentication;
using Eawv.Service.Exceptions;
using Eawv.Service.Models;
using Eawv.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voting.Lib.Iam.Services.ApiClient.Identity;

namespace Eawv.Service.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController
{
    private readonly IUserService _userService;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, AuthService authService, IMapper mapper)
    {
        _userService = userService;
        _authService = authService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<IEnumerable<UserModel>> GetUsers()
    {
        var tenantId = _authService.GetTenantId();
        return _mapper.Map<IEnumerable<UserModel>>(await _userService.GetUsersFromChildTenants(tenantId));
    }

    [HttpGet("tenant/{tenantId}")]
    [Authorize(Roles = Role.All)]
    public async Task<IEnumerable<UserModel>> GetUsersForTenant(string tenantId)
    {
        var users = _mapper.Map<List<UserModel>>(await _userService.GetUsersForTenant(tenantId));

        foreach (var user in users)
        {
            ScrubFields(user);
        }

        return users;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = Role.All)]
    public async Task<UserModel> GetUser(string id)
    {
        var user = await _userService.Get(id);
        var auths = await _userService.GetParentAuthorizationsForUser(id);
        if (auths.Count == 0)
        {
            throw new BadRequestException("User does not belong to current or child tenant");
        }

        var tenantUser = new TenantUser(user, auths);
        var result = _mapper.Map<UserModel>(tenantUser);
        ScrubFields(result);
        return result;
    }

    [HttpPost]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<UserModel> CreateUser([FromBody] CreateUserModel userModel)
    {
        userModel.Email = userModel.Email.Trim();
        userModel.Username = userModel.Username.Trim();
        var user = _mapper.Map<V1User>(userModel);
        var tenantUser = await _userService.CreateUser(user, userModel.Email, userModel.Tenants);
        return _mapper.Map<UserModel>(tenantUser);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<UserModel> UpdateUser(string id, [FromBody] ModifyUserModel userModel)
    {
        var tenantUser = await _userService.UpdateUser(id, userModel.Tenants);
        return _mapper.Map<UserModel>(tenantUser);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task RemoveUserAccess(string id)
    {
        await _userService.RemoveRelevantAuthorizations(id);
    }

    private void ScrubFields(UserModel user)
    {
        if (_authService.IsWahlverwalter)
        {
            return;
        }

        user.Username = string.Empty;
    }
}
