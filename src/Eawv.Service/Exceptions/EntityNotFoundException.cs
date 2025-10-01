// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net;

namespace Eawv.Service.Exceptions;

[Serializable]
public class EntityNotFoundException : Exception, IHttpStatusCodeException
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(Guid id)
        : base($"The entity with id {id} could not be found.")
    {
    }

    public EntityNotFoundException(Guid id, string tenantId)
        : base($"The entity with the id {id} and tenantId {tenantId} could not be found.")
    {
    }

    public EntityNotFoundException(params Guid[] ids)
        : base($"The entity with the combined ids {string.Join(" and ", ids)} could not be found.")
    {
    }

    public EntityNotFoundException(string name)
        : base($"The entity {name} could not be found.")
    {
    }

    public HttpStatusCode GetHttpStatusCode()
    {
        return HttpStatusCode.NotFound;
    }
}
