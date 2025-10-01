// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Models;

public class ModifyListModel
{
    [Required]
    [MaxLength(50)]
    [SimpleSlText]
    public string ResponsiblePartyTenantId { get; set; }

    [MaxLength(10)]
    [SimpleSlText]
    public string Indenture { get; set; }

    public DateTime? SubmitDate { get; set; }

    [Required]
    [MaxLength(100)]
    [ComplexSlText]
    public string Name { get; set; }

    [MaxLength(100)]
    [ComplexSlText]
    public string Description { get; set; }

    public int? SortOrder { get; set; }

    public bool Validated { get; set; }

    public bool Locked { get; set; }

    [Required]
    [MaxLength(50)]
    [SimpleSlText]
    public string Representative { get; set; }

    [MaxLength(1)]
    public List<string> DeputyUsers { get; set; }

    [MaxLength(2)]
    public List<string> MemberUsers { get; set; }
}
