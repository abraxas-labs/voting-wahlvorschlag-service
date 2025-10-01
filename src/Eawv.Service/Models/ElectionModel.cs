// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Eawv.Service.DataAccess.Entities;

namespace Eawv.Service.Models;

public class ElectionModel : BaseEntityModel
{
    public string TenantId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime SubmissionDeadlineBegin { get; set; }

    public DateTime SubmissionDeadlineEnd { get; set; }

    public DateTime ContestDate { get; set; }

    public DateTime? AvailableFrom { get; set; }

    public ElectionType ElectionType { get; set; }

    public int? QuorumSignaturesCount { get; set; }

    public int State { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1819:Properties should not return arrays",
        Justification = "EF Core property that maps a byte array from the database must be of type byte[].")]
    public byte[] TenantLogo { get; set; }

    public IList<EmptyBallotDocumentModel> Documents { get; set; }

    public IList<DomainOfInfluenceElectionModel> DomainsOfInfluence { get; set; }

    public IList<InfoTextModel> InfoTexts { get; set; }

    public bool IsArchived { get; set; }
}
