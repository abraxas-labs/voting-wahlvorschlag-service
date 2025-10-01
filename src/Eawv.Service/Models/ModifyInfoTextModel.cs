// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Models;

public class ModifyInfoTextModel
{
    public Guid? ElectionId { get; set; }

    [MaxLength(50)]
    public string TenantId { get; set; }

    [Required]
    [MaxLength(50)]
    [SimpleSlText]
    public string Key { get; set; }

    [MaxLength(10_000)]
    [ComplexMlText]
    public string Value { get; set; }

    public bool Visible { get; set; } = true;
}
