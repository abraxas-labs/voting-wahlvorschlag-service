// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Configuration;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Eawv.Service.Models.TemplateServiceModels;
using Eawv.Service.Services;
using Eawv.Service.Unit.Tests.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Voting.Lib.Iam.Store;
using Voting.Lib.Iam.TokenHandling;
using Voting.Lib.Testing.Mocks;
using Xunit;

namespace Eawv.Service.Unit.Tests;

public class NotificationServiceUnitTest
{
    private const string EmailSenderAddress = "no-reply-local@abraxas.ch";
    private const string NotificationEndpoint = "https://sta.corp.abraxas-apis.ch/notify/";
    private const TemplateType NewCommentTemplateType = TemplateType.EmailListNewComment;

    private static readonly TemplateBag TemplateBag = new TemplateBag
    {
        List = new List(),
        Election = new Election(),
    };

    private readonly List<string> _recipientLoginIds = new List<string> { "362747924083418558" };
    private readonly Mock<ILogger<NotificationService>> _logger;
    private readonly AuthService _authService;
    private readonly Mock<ITemplateService> _templateService;
    private readonly NotificationServiceConfiguration _config;

    public NotificationServiceUnitTest()
    {
        _logger = GetLoggerMock();
        _authService = GetAuthServiceMock();
        _templateService = GetTemplateServiceMock();
        _config = GetNotificationServiceConfigurationMock();
    }

    [Fact]
    public async Task ShouldThrowEmailNotificationExceptionWhenIsNotSuccessStatusCode()
    {
        var httpFact = new Mock<IHttpClientFactory>();
        var httpClient = new HttpClient();
        httpFact.Setup(x => x.CreateClient(string.Empty)).Returns(httpClient).Verifiable();

        var notificationService = new NotificationService(httpFact.Object, _authService, _templateService.Object, _config, _logger.Object);
        await Assert.ThrowsAsync<EmailNotificationException>(() => notificationService.SendEmailAsync(_recipientLoginIds, NewCommentTemplateType, TemplateBag));

        httpClient.Dispose();
    }

    [Fact]
    public async Task ShouldLogEmailSentWhenIsSuccessStatusCode()
    {
        var httpFact = new Mock<IHttpClientFactory>();

        var httpResponseMessage = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("success"),
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(httpResponseMessage)
           .Verifiable();

        var client = new HttpClient(handlerMock.Object);

        httpFact.Setup(x => x.CreateClient(string.Empty)).Returns(client).Verifiable();

        var notificationService = new NotificationService(httpFact.Object, _authService, _templateService.Object, _config, _logger.Object);

        await notificationService.SendEmailAsync(_recipientLoginIds, NewCommentTemplateType, TemplateBag);

        _logger.VerifyLogging($"Email sent to recipients. to: bcc:{string.Join(", ", _recipientLoginIds)}", LogLevel.Information, Times.Once());

        httpResponseMessage.Dispose();
        client.Dispose();
    }

    private static Mock<ILogger<NotificationService>> GetLoggerMock()
    {
        return new Mock<ILogger<NotificationService>>();
    }

    private static NotificationServiceConfiguration GetNotificationServiceConfigurationMock()
    {
        return new NotificationServiceConfiguration
        {
            Endpoint = new Uri(NotificationEndpoint),
            SenderEmail = EmailSenderAddress,
        };
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

    private static AuthService GetAuthServiceMock()
    {
        var auth = new Mock<IAuth>();
        var serviceTokenHandler = new Mock<ITokenHandler>();
        serviceTokenHandler.Setup(x => x.GetToken(CancellationToken.None)).ReturnsAsync(string.Empty);

        return new AuthService(serviceTokenHandler.Object, auth.Object, new MockedClock());
    }
}
