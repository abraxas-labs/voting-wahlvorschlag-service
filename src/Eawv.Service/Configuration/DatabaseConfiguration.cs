// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Npgsql;

namespace Eawv.Service.Configuration;

public class DatabaseConfiguration
{
    public string ConnectionString => new NpgsqlConnectionStringBuilder
    {
        Host = Host,
        Port = Port,
        Database = Database,
        Username = User,
        Password = Password,
        IncludeErrorDetail = EnableDetailedErrors,
        CommandTimeout = CommandTimeout,
    }.ToString();

    public string Host { get; set; } = string.Empty;

    public ushort Port { get; set; } = 5432;

    public string User { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Database { get; set; } = string.Empty;

    public bool EnableSensitiveDataLogging { get; set; }

    public bool EnableDetailedErrors { get; set; }

    /// <summary>
    /// Gets or sets the command timeout for database queries in seconds.
    /// Framework default is 30 sec.
    /// </summary>
    public ushort CommandTimeout { get; set; } = 30;
}
