// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Exceptions;

[Serializable]
public class TooManyCandidatesException : BadRequestException
{
    public TooManyCandidatesException()
    {
    }

    public TooManyCandidatesException(int candidatesCount, int mandates)
        : base($"Cannot have {candidatesCount} candidates for an election with {mandates} mandates")
    {
    }
}
