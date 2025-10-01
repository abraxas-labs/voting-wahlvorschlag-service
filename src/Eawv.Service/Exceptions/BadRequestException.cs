// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net;

namespace Eawv.Service.Exceptions;

[Serializable]
public class BadRequestException : Exception, IHttpStatusCodeException
{
    public BadRequestException()
    {
    }

    public BadRequestException(string msg)
        : base(msg)
    {
    }

    public HttpStatusCode GetHttpStatusCode()
    {
        return HttpStatusCode.BadRequest;
    }
}
