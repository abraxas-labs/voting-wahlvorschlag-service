// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Eawv.Service.DataAccess.Entities;

namespace Eawv.Service.Models;

public class ListModel : BaseEntityModel
{
    public string ResponsiblePartyTenantId { get; set; }

    public string Indenture { get; set; }

    public DateTime? SubmitDate { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int SortOrder { get; set; }

    public ListState State { get; set; }

    public string Representative { get; set; }

    public List<string> DeputyUsers { get; set; }

    public List<string> MemberUsers { get; set; }

    public bool Validated { get; set; }

    public bool Locked { get; set; }

    public int Version { get; set; }

    public IList<CandidateModel> Candidates { get; set; }

    public ListUnionModel ListUnion { get; set; }

    public ListUnionModel ListSubUnion { get; set; }

    public string CreatedByName { get; set; }

    public string ModifiedByName { get; set; }
}
