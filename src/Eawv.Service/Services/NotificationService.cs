// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Configuration;
using Eawv.Service.Exceptions;
using Eawv.Service.Models.NotificationServiceModels;
using Eawv.Service.Models.TemplateServiceModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Eawv.Service.Services;

/// <inheritdoc cref="INotificationService"/>
public class NotificationService : INotificationService
{
    private const string PathEmail = "email";

    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;
    private readonly ITemplateService _templateService;
    private readonly ILogger<NotificationService> _logger;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    private readonly NotificationServiceConfiguration _notificationServiceConfiguration;

    public NotificationService(
        IHttpClientFactory httpFact,
        AuthService authService,
        ITemplateService templateService,
        NotificationServiceConfiguration config,
        ILogger<NotificationService> logger)
    {
        _httpClient = httpFact.CreateClient();
        _authService = authService;
        _templateService = templateService;
        _logger = logger;
        _httpClient.BaseAddress = config.Endpoint;
        _notificationServiceConfiguration = config;

        _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };
    }

    public async Task SendEmailAsync(List<string> recipientLoginIds, TemplateType type, TemplateBag bag)
    {
        var renderResult = await _templateService.RenderToHtml(type, bag, true);
        var content = await renderResult.ReadAsString();
        await SendEmailAsync(recipientLoginIds, renderResult.Filename, content);
    }

    public async Task SendEmailAsync(SendEmailRequestModel requestModel)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, PathEmail);
        var requestContentJson = JsonConvert.SerializeObject(requestModel, _jsonSerializerSettings);
        using var requestContent = new StringContent(requestContentJson, Encoding.UTF8, "application/json");
        httpRequest.Content = requestContent;

        var token = await _authService.GetServiceToken();
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

        using var httpResponse = await _httpClient.SendAsync(httpRequest);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = httpResponse.Content == null
                ? string.Empty
                : await httpResponse.Content.ReadAsStringAsync();

            throw new EmailNotificationException($"Failed to send email: {httpResponse.StatusCode} - {errorMessage}");
        }

        _httpClient.Dispose();
        var recipientsTo = string.Join(',', requestModel.Recipients?.Select(r => r.LoginId) ?? []);
        var recipientBcc = string.Join(',', requestModel.Bcc?.Select(r => r.LoginId) ?? []);
        _logger.LogInformation("Email sent to recipients. to:{recipientsTo} bcc:{recipientBcc}", recipientsTo, recipientBcc);
    }

    private async Task SendEmailAsync(List<string> recipientLoginIds, string subject, string content)
    {
        await SendEmailAsync(new SendEmailRequestModel
        {
            Bcc = recipientLoginIds.Distinct().Select(id => new RecipientModel { LoginId = id }).ToList(),
            Message = new MessageModel
            {
                Subject = new MessageContentModel
                {
                    Raw = subject,
                },
                Content = new MessageContentModel
                {
                    Raw = content,
                },
            },
            Sender = new EmailSenderModel
            {
                DisplayName = _notificationServiceConfiguration.SenderEmail,
            },
        });
    }
}
