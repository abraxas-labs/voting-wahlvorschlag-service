// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.Configuration;

/// <summary>
/// Ports settings from the appsettings.json.
/// </summary>
public class PortConfig
{
    /// <summary>
    /// Gets or Sets the Http Port.
    /// </summary>
    public ushort Http { get; set; } = 5000;

    /// <summary>
    /// Gets or Sets the Http2 Port.
    /// </summary>
    public ushort Http2 { get; set; } = 5002;
}
