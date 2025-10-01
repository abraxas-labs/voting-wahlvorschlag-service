// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Models;

public class ModifyUserModel
{
    [Required]
    [MaxLength(100)]
    [ComplexSlText]
    public string Firstname { get; set; }

    [Required]
    [MaxLength(100)]
    [ComplexSlText]
    public string Lastname { get; set; }

    [MaxLength(50)]
    public List<string> Tenants { get; set; }
}
