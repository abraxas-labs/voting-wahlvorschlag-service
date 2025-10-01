// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.Integration.Tests.MockedData;

public static class DatabaseUtil
{
    private static bool _migrated;

    public static async Task<bool> EnsureMigrated(EawvContext db)
    {
        if (_migrated)
        {
            return false;
        }

        await db.Database.MigrateAsync();
        _migrated = true;
        return true;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1002:Risk of vulnerability to SQL injection.", Justification = "Referencing hardened inerpolated string parameters.")]
    public static async Task Truncate(EawvContext db)
    {
        // truncating tables is much faster than recreating the database
        // fastest would probably be postgres template db's with our mock data, but they don't work easily with
        // x unit test parallelization
        await EnsureMigrated(db);

        // truncating tables is much faster than recreating the database
        var tableNames = db.Model.GetEntityTypes()
            .Where(t => !t.GetTableName().Equals("Templates", System.StringComparison.InvariantCultureIgnoreCase))
            .Select(m => $@"""{m.GetTableName()}""");
        await db.Database.ExecuteSqlRawAsync($"TRUNCATE {string.Join(",", tableNames)} CASCADE");
    }
}
