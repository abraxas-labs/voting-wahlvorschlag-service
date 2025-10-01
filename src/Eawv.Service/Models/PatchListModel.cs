// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Eawv.Service.DataAccess.Entities;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Models;

public class PatchListModel
{
    [ValidEnum]
    public ListState State { get; set; }
}
