// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Models;
using Eawv.Service.Models.TemplateServiceModels;
using Microsoft.Extensions.Logging;
using Voting.Lib.Iam.Services.ApiClient.Identity;

namespace Eawv.Service.Services;

public class EventNotificationService
{
    private readonly AuthService _authService;
    private readonly INotificationService _notificationService;
    private readonly IUserService _userService;
    private readonly ILogger<EventNotificationService> _logger;

    public EventNotificationService(
        AuthService authService,
        INotificationService notificationService,
        IUserService userService,
        ILogger<EventNotificationService> logger)
    {
        _authService = authService;
        _notificationService = notificationService;
        _userService = userService;
        _logger = logger;
    }

    public async Task ListStateChanged(Election election, List list, Theme theme)
    {
        await SendNotification(election, list, TemplateType.EmailListStateChanged, theme);
    }

    public async Task ListNewComment(Election election, List list, Theme theme)
    {
        await SendNotification(election, list, TemplateType.EmailListNewComment, theme);
    }

    public async Task SendNotification(Election election, List list, TemplateType type, Theme theme)
    {
        var users = new List<string>
        {
            election.CreatedBy,
            election.ModifiedBy,
            list.CreatedBy,
            list.ModifiedBy,
        };

        var eawvUsers = await _userService.GetWahlverwaltersForTenant(election.TenantId);
        users.AddRange(eawvUsers.Select(u => u.Loginid));

        users.Add(list.Representative);
        users.AddRange(list.DeputyUsers);

        users = users.Distinct()
            .Where(id => id != null)
            .ToList();
        users.Remove(_authService.GetUserId());

        var emails = new HashSet<string>();
        for (var i = users.Count - 1; i >= 0; i--)
        {
            try
            {
                var user = await _userService.Get(users[i]);
                var isDeleted = user.Lifecycle == ApiStorageLifecycle.DELETED;
                var primaryEmail = user.Emails?.FirstOrDefault(e => e.Primary == true)?.Email;
                if (isDeleted || string.IsNullOrEmpty(primaryEmail) || !emails.Add(primaryEmail))
                {
                    users.RemoveAt(i);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "User with id {id} doesn't exist and won't get an email.", users[i]);
                users.RemoveAt(i);
            }
        }

        if (users.Count == 0)
        {
            return;
        }

        await _notificationService.SendEmailAsync(users, type, new TemplateBag
        {
            List = list,
            Election = election,
            Theme = theme.ToString().ToLower(),
        });
    }
}
