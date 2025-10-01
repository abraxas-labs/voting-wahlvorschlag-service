// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Net;

namespace Eawv.Service.Exceptions;

public interface IHttpStatusCodeException
{
    HttpStatusCode GetHttpStatusCode();
}
