// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Models;

public class MarkedElementModel : BaseEntityModel
{
    public string Field { get; set; }

    public Guid CandidateId { get; set; }
}
