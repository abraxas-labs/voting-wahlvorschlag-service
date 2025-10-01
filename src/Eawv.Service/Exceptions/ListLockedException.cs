// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Exceptions;

[Serializable]
public class ListLockedException : BadRequestException
{
    public ListLockedException()
    {
    }

    public ListLockedException(Guid listId)
        : base($"List with id {listId} is locked")
    {
    }
}
