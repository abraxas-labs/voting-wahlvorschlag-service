// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Exceptions;

public class DuplicateListIndentureException : BadRequestException
{
    public DuplicateListIndentureException(Guid electionId, string listIndenture)
        : base($"The list indenture {listIndenture} for election {electionId} already exists.")
    {
    }
}
