// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Eawv.Service.Integration.Tests.MockedData;

public class MockedUser
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public Dictionary<string, string> RoleByTenantId { get; set; }
}
