// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Eawv.Service.Converters;
using Eawv.Service.DataAccess.Entities;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Models;

public class ModifyCandidateModel
{
    /// <summary>
    /// Gets or sets the Id for batch updates, to check whether this is an existing item.
    /// </summary>
    [JsonConverter(typeof(GuidNullValueJsonConverter))]
    public Guid? Id { get; set; }

    [Required]
    [ComplexSlText]
    [MaxLength(100)]
    public string FamilyName { get; set; }

    [Required]
    [ComplexSlText]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [ComplexSlText]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    [ComplexSlText]
    [MaxLength(250)]
    public string OccupationalTitle { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [ValidEnum]
    public SexType Sex { get; set; }

    [ComplexSlText]
    [MaxLength(80)]
    public string Origin { get; set; }

    [Required]
    [ComplexSlText]
    [MaxLength(150)]
    public string Street { get; set; }

    [AlphaNumWhite]
    [MaxLength(30)]
    public string HouseNumber { get; set; }

    [Required]
    [ComplexSlText]
    [MaxLength(20)]
    public string ZipCode { get; set; }

    [Required]
    [ComplexSlText]
    [MaxLength(200)]
    public string Locality { get; set; }

    [Required]
    [ComplexSlText]
    [MaxLength(100)]
    public string BallotFamilyName { get; set; }

    [Required]
    [ComplexSlText]
    [MaxLength(100)]
    public string BallotFirstName { get; set; }

    [Required]
    [ComplexSlText]
    [MaxLength(100)]
    public string BallotOccupationalTitle { get; set; }

    [Required]
    [ComplexSlText]
    [MaxLength(200)]
    public string BallotLocality { get; set; }

    [Required]
    public bool Incumbent { get; set; }

    public bool Cloned { get; set; }

    [Required]
    public int Index { get; set; }

    [Required]
    public int OrderIndex { get; set; }

    public int? CloneOrderIndex { get; set; }

    /// <summary>
    /// Gets or sets the eCH partyNameShort free text (max. 12 characters).
    /// </summary>
    [ComplexSlText]
    [MaxLength(12)]
    public string Party { get; set; }

    [MaxLength(40)]
    public List<ModifyMarkedElementModel> Markings { get; set; }
}
