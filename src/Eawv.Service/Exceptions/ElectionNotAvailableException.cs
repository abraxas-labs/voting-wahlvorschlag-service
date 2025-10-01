// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Exceptions;

[Serializable]
public class ElectionNotAvailableException : BadRequestException
{
    public ElectionNotAvailableException()
    {
    }

    public ElectionNotAvailableException(Guid electionId, DateTime availableFrom)
        : base($"Election with id {electionId} is not yet available. Availability date: {availableFrom}")
    {
    }
}
