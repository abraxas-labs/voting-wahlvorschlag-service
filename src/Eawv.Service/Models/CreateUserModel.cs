// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Models;

public class CreateUserModel
{
    [Required]
    [ComplexSlText]
    [MaxLength(50)]
    public string Firstname { get; set; }

    [Required]
    [ComplexSlText]
    [MaxLength(50)]
    public string Lastname { get; set; }

    [Required]
    [ComplexSlText]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; }

    [MaxLength(50)]
    public List<string> Tenants { get; set; }
}
