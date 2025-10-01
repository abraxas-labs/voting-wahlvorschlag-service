// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.ComponentModel.DataAnnotations;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Models;

public class ModifyListCommentModel
{
    [MaxLength(1000)]
    [ComplexMlText]
    public string Content { get; set; }
}
