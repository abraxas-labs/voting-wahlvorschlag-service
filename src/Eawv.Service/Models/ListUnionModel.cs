// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Eawv.Service.Models;

public class ListUnionModel
{
    public Guid? RootListId { get; set; }

    public IList<IdModel> Lists { get; set; }

    public bool IsSubUnion => RootListId.HasValue;
}
