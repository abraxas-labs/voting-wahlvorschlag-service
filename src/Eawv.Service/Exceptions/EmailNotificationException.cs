// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Exceptions;

/// <summary>
/// Exception that can be thrown if an E-Mail send via SEAS Notification Service wasn't successful.
/// </summary>
[Serializable]
public class EmailNotificationException : Exception
{
    public EmailNotificationException()
    {
    }

    public EmailNotificationException(string message)
        : base(message)
    {
    }
}
