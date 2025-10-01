// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Eawv.Service.DataAccess.Entities;

namespace Eawv.Service.Models;

public class DomainOfInfluenceModel : BaseEntityModel
{
    public string TenantId { get; set; }

    public string OfficialId { get; set; }

    public string Name { get; set; }

    public string ShortName { get; set; }

    public DomainOfInfluenceType DomainOfInfluenceType { get; set; }
}
