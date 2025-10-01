// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Configuration;

/// <summary>
/// Configuration for the SEAS Notification Service.
/// </summary>
public class NotificationServiceConfiguration
{
    public const string SectionName = "NotificationService";

    /// <summary>
    /// Gets or Sets the Notification Service Endpoint URI including also the version path.
    /// </summary>
    public Uri Endpoint { get; set; }

    /// <summary>
    /// Gets or Sets the sender email address used for the notification email, i.e. 'no-reply@abraxas.ch'.
    /// </summary>
    public string SenderEmail { get; set; }

    /// <summary>
    /// Gets or sets the support email address used for domain specific requests, i.e. 'voting@abraxas.ch'.
    /// </summary>
    public string SupportEmail { get; set; }
}
