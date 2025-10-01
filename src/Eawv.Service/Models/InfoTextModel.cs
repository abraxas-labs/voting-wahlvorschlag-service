// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Models;

public class InfoTextModel : BaseEntityModel
{
    public Guid? ElectionId { get; set; }

    public string TenantId { get; set; }

    public string Key { get; set; }

    public string Value { get; set; }

    public bool Visible { get; set; }
}
