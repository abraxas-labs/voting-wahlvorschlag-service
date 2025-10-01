// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net;

namespace Eawv.Service.Exceptions;

[Serializable]
public class ForbiddenException : Exception, IHttpStatusCodeException
{
    public ForbiddenException()
        : base("Insufficient rights to access this resource")
    {
    }

    public ForbiddenException(Guid id)
        : base($"Insufficient rights to access {id}.")
    {
    }

    public ForbiddenException(string msg)
        : base(msg)
    {
    }

    public HttpStatusCode GetHttpStatusCode()
    {
        return HttpStatusCode.Forbidden;
    }
}
