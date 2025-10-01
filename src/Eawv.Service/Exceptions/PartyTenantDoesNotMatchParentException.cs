// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Exceptions;

[Serializable]
public class PartyTenantDoesNotMatchParentException : BadRequestException
{
    public PartyTenantDoesNotMatchParentException()
    {
    }

    public PartyTenantDoesNotMatchParentException(string partyId, string parentId)
        : base($"The party with id {partyId} is not a child of {parentId}")
    {
    }
}
