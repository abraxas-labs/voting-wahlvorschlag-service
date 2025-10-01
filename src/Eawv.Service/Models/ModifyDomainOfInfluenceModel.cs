// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.ComponentModel.DataAnnotations;
using Eawv.Service.DataAccess.Entities;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Models;

public class ModifyDomainOfInfluenceModel
{
    [Required]
    [SimpleSlText]
    [MaxLength(50)]
    public string OfficialId { get; set; }

    [Required]
    [SimpleSlText]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [SimpleSlText]
    [MaxLength(50)]
    public string ShortName { get; set; }

    [ValidEnum]
    public DomainOfInfluenceType DomainOfInfluenceType { get; set; }
}
