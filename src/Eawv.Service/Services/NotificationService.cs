// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eawv.Service.Models.NotificationServiceModels;
using Eawv.Service.Models.TemplateServiceModels;
using Microsoft.Extensions.Logging;
using Voting.Lib.UserNotifications;

namespace Eawv.Service.Services;

/// <inheritdoc cref="INotificationService"/>
public class NotificationService : INotificationService
{
    private readonly IUserNotificationSender _notificationSender;
    private readonly ITemplateService _templateService;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IUserNotificationSender notificationSender,
        ITemplateService templateService,
        ILogger<NotificationService> logger)
    {
        _notificationSender = notificationSender;
        _templateService = templateService;
        _logger = logger;
    }

    public async Task SendEmailAsync(List<string> recipientEmails, TemplateType type, TemplateBag bag)
    {
        var renderResult = await _templateService.RenderToHtml(type, bag, true);
        var content = await renderResult.ReadAsString();

        await SendEmailAsync(new SendEmailRequestModel
        {
            Recipients = recipientEmails.Distinct().Select(email => new RecipientModel { EmailAddress = email }).ToList(),
            Message = new MessageModel
            {
                Subject = new MessageContentModel
                {
                    Raw = renderResult.Filename,
                },
                Content = new MessageContentModel
                {
                    Raw = content,
                },
            },
        });
    }

    public async Task SendEmailAsync(SendEmailRequestModel requestModel)
    {
        var recipients = (requestModel.Recipients ?? [])
            .Select(r => r.EmailAddress)
            .Where(email => !string.IsNullOrEmpty(email))
            .Distinct()
            .ToList();

        foreach (var recipient in recipients)
        {
            await _notificationSender.Send(
                new UserNotification(recipient, requestModel.Message.Subject.Raw, requestModel.Message.Content.Raw),
                CancellationToken.None);
        }

        _logger.LogInformation("Email sent to recipients. to:{recipients}", string.Join(',', recipients));
    }
}
