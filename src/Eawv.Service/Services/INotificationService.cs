// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading.Tasks;
using Eawv.Service.Models.NotificationServiceModels;
using Eawv.Service.Models.TemplateServiceModels;

namespace Eawv.Service.Services;

/// <summary>
/// Service for sending notifications using the SEAS Notification Service.
/// Used version / endpoint and genaral configuration see <see cref="NotificationServiceConfiguration"/>.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Renders the template and sends it as an email via the notification service.
    /// The filename of the template is used as subject.
    /// </summary>
    /// <param name="recipientLoginIds">The Secure Connect login IDs of the targeted recipients which should receive an email.</param>
    /// <param name="type">The template type for the email.</param>
    /// <param name="bag">The template data model.</param>
    /// <returns>Task for async handling.</returns>
    Task SendEmailAsync(List<string> recipientLoginIds, TemplateType type, TemplateBag bag);

    /// <summary>
    /// Sends an email using the provided request model.
    /// </summary>
    /// <param name="requestModel">The model containing the email details to be sent.</param>
    /// <returns>Task for async handling.</returns>
    Task SendEmailAsync(SendEmailRequestModel requestModel);
}
