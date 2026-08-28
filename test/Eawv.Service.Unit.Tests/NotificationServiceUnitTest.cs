// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Models.TemplateServiceModels;
using Eawv.Service.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Voting.Lib.UserNotifications;
using Xunit;

namespace Eawv.Service.Unit.Tests;

public class NotificationServiceUnitTest
{
    private const TemplateType NewCommentTemplateType = TemplateType.EmailListNewComment;

    private static readonly TemplateBag TemplateBag = new TemplateBag
    {
        List = new List(),
        Election = new Election(),
    };

    private readonly List<string> _recipientEmails = new List<string> { "test@example.invalid" };
    private readonly Mock<ILogger<NotificationService>> _logger;
    private readonly Mock<ITemplateService> _templateService;
    private readonly Mock<IUserNotificationSender> _notificationSender;

    public NotificationServiceUnitTest()
    {
        _logger = GetLoggerMock();
        _templateService = GetTemplateServiceMock();
        _notificationSender = new Mock<IUserNotificationSender>();
    }

    [Fact]
    public async Task ShouldSendNotificationToEachRecipient()
    {
        var notificationService = new NotificationService(_notificationSender.Object, _templateService.Object, _logger.Object);

        await notificationService.SendEmailAsync(_recipientEmails, NewCommentTemplateType, TemplateBag);

        _notificationSender.Verify(
            x => x.Send(It.Is<UserNotification>(n => n.RecipientEmail == _recipientEmails[0]), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ShouldPropagateExceptionWhenSendFails()
    {
        _notificationSender
            .Setup(x => x.Send(It.IsAny<UserNotification>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new SmtpException());

        var notificationService = new NotificationService(_notificationSender.Object, _templateService.Object, _logger.Object);

        await Assert.ThrowsAsync<SmtpException>(() => notificationService.SendEmailAsync(_recipientEmails, NewCommentTemplateType, TemplateBag));
    }

    private static Mock<ILogger<NotificationService>> GetLoggerMock()
    {
        return new Mock<ILogger<NotificationService>>();
    }

    private static Mock<ITemplateService> GetTemplateServiceMock()
    {
        var templateService = new Mock<ITemplateService>();
        templateService
            .Setup(x => x.RenderToHtml(It.IsAny<TemplateType>(), It.IsAny<TemplateBag>(), It.IsAny<bool>()))
            .ReturnsAsync(new RenderResult(
                "Notification Service Unit Test",
                MediaTypeNames.Text.Html,
                _ => Task.CompletedTask));
        return templateService;
    }
}
