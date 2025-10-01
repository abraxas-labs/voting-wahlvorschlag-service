// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Eawv.Service.Models;

public class UserModel
{
    public string Id { get; set; }

    public string Loginid { get; set; }

    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public string Username { get; set; }

    public List<PartyModel> Tenants { get; set; }
}
