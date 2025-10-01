// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.Models.NotificationServiceModels;
using Eawv.Service.Models.TemplateServiceModels;
using Eawv.Service.Services;

namespace Eawv.Service.Integration.Tests.Mocks;

public class NotificationServiceMock : INotificationService
{
    private readonly ITemplateService _templateService;

    public NotificationServiceMock(ITemplateService templateService)
    {
        _templateService = templateService;
    }

    public static List<SendEmailRequestModel> SentEmails { get; } = [];

    public async Task SendEmailAsync(List<string> recipientLoginIds, TemplateType type, TemplateBag bag)
    {
        var renderResult = await _templateService.RenderToHtml(type, bag, true);
        var content = await renderResult.ReadAsString();
        await SendEmailAsync(new SendEmailRequestModel
        {
            Bcc = recipientLoginIds.Distinct().Select(id => new RecipientModel { LoginId = id }).ToList(),
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

    public Task SendEmailAsync(SendEmailRequestModel requestModel)
    {
        // Replace Windows line endings to make this work in the CI
        requestModel.Message.Content.Raw = requestModel.Message.Content.Raw.ReplaceLineEndings("\n");

        SentEmails.Add(requestModel);
        return Task.CompletedTask;
    }
}
