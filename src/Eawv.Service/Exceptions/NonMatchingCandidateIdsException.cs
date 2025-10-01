// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Eawv.Service.Exceptions;

[Serializable]
public class NonMatchingCandidateIdsException : BadRequestException
{
    public NonMatchingCandidateIdsException()
    {
    }

    public NonMatchingCandidateIdsException(List<Guid> ids)
        : base($"Candidate ids count does not match the database count. Values: {string.Join(',', ids)}")
    {
    }
}
