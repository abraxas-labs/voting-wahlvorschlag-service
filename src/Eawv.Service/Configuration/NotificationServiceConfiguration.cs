// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.Configuration;

/// <summary>
/// Business configuration for notification emails sent by the <see cref="Services.NotificationService"/>.
/// </summary>
public class NotificationServiceConfiguration
{
    public const string SectionName = "NotificationService";

    /// <summary>
    /// Gets or sets the support email address used for domain specific requests, i.e. 'voting@abraxas.ch'.
    /// </summary>
    public string SupportEmail { get; set; }
}
