// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;

namespace Eawv.Service.Models;

public class CreateDomainOfInfluenceElectionModel
{
    [Required]
    public Guid Id { get; set; }

    [Range(1, 200)]
    public int NumberOfMandates { get; set; }
}
