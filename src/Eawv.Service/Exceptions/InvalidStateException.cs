// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Exceptions;

[Serializable]
public class InvalidStateException : BadRequestException
{
    public InvalidStateException()
    {
    }

    public InvalidStateException(object old, object newState)
        : base($"State transition from {old} to {newState} is not allowed")
    {
    }
}
