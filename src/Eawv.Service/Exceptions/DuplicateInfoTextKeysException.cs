// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Exceptions;

[Serializable]
public class DuplicateInfoTextKeysException : BadRequestException
{
    public DuplicateInfoTextKeysException()
        : base("The info texts contain duplicate keys.")
    {
    }
}
