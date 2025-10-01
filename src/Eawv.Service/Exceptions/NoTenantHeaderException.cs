// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Exceptions;

[Serializable]
public class NoTenantHeaderException : BadRequestException
{
    public NoTenantHeaderException()
        : base("No tenant header was found in the request.")
    {
    }
}
