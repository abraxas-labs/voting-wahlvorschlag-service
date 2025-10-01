// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eawv.Service.Models;

namespace Eawv.Service.Services;

/// <summary>
/// Service serving election list data.
/// </summary>
public interface IListService
{
    /// <summary>
    /// Gets a sorted collection of election lists.
    /// </summary>
    /// <param name="electionId">The election id.</param>
    /// <returns>A <see cref="Task"/> returning a collection of election lists.</returns>
    public Task<IEnumerable<ListModel>> GetLists(Guid electionId);
}
