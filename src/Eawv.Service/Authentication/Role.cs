// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Eawv.Service.Authentication;

public static class Role
{
    public const string Wahlverwalter = "Wahlverwalter";

    public const string User = "Benutzer";

    public const string All = Wahlverwalter + Separator + User;

    private const string Separator = ",";

    public static IEnumerable<string> AllRoles()
    {
        yield return Wahlverwalter;
        yield return User;
    }
}
