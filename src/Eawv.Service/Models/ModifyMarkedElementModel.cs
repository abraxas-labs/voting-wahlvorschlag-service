// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Models;

public class ModifyMarkedElementModel
{
    public Guid? Id { get; set; }

    [MaxLength(100)]
    [SimpleSlText]
    public string Field { get; set; }
}
