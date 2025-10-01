// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Models;

public class EmptyBallotDocumentModel : BaseEntityModel
{
    public string Name { get; set; }

    public Guid ElectionId { get; set; }
}
