// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.ComponentModel.DataAnnotations;

namespace Eawv.Service.Models;

public class UpdateDomainOfInfluenceElectionModel
{
    [Range(1, 200)]
    public int NumberOfMandates { get; set; }
}
