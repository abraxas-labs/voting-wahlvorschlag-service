// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Eawv.Service.DataAccess.Entities;

namespace Eawv.Service.Models;

public class CandidateModel : BaseEntityModel
{
    public string FamilyName { get; set; }

    public string FirstName { get; set; }

    public string Title { get; set; }

    public string OccupationalTitle { get; set; }

    public DateTime DateOfBirth { get; set; }

    public SexType Sex { get; set; }

    public string Origin { get; set; }

    public string Street { get; set; }

    public string HouseNumber { get; set; }

    public string ZipCode { get; set; }

    public string Locality { get; set; }

    public string BallotFamilyName { get; set; }

    public string BallotFirstName { get; set; }

    public string BallotOccupationalTitle { get; set; }

    /// <summary>
    /// Gets or sets the political locality for the ballot details.
    /// </summary>
    public string BallotLocality { get; set; }

    public bool Incumbent { get; set; }

    public Guid ListId { get; set; }

    public bool Cloned { get; set; }

    public int Index { get; set; }

    /// <summary>
    /// Gets or Sets the OrderIndex for background sorting.
    /// </summary>
    public int OrderIndex { get; set; }

    /// <summary>
    /// Gets or Sets the ClonOrderIndex. This is needed for sorting the preCumulated Candidates.
    /// </summary>
    public int? CloneOrderIndex { get; set; }

    /// <summary>
    /// Gets or sets the eCH partyNameShort free text (max. 12 characters).
    /// </summary>
    public string Party { get; set; }

    public IList<MarkedElementModel> Markings { get; set; }
}
