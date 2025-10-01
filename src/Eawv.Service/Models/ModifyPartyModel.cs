// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.ComponentModel.DataAnnotations;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Models;

public class ModifyPartyModel
{
    [Required]
    [MaxLength(100)]
    [ComplexSlText]
    public string Name { get; set; }
}
