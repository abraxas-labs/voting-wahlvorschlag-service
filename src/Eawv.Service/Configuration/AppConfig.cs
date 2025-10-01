// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Eawv.Service.Ech.Configuration;
using Voting.Lib.Iam.Configuration;
using Voting.Lib.Iam.Services;
using Voting.Lib.MalwareScanner.Configuration;

namespace Eawv.Service.Configuration;

/// <summary>
/// Represents the appsettings.json structure.
/// </summary>
public class AppConfig
{
    /// <summary>
    /// Gets or Sets the allowed hosts from the appsettings.
    /// </summary>
    public string AllowedHosts { get; set; }

    /// <summary>
    /// Gets or Sets the cache settings from the appsettings.
    /// </summary>
    public CacheConfiguration Cache { get; set; }

    /// <summary>
    /// Gets or Sets the database settings from the appsettings.
    /// </summary>
    public DatabaseConfiguration Database { get; set; }

    /// <summary>
    /// Gets or Sets the GUIUrl from the appsettings, which means the url to the angular web application.
    /// </summary>
    public string GUIUrl { get; set; }

    /// <summary>
    /// Gets or Sets the database settings from the appsettings.
    /// </summary>
    public NotificationServiceConfiguration NotificationService { get; set; }

    /// <summary>
    /// Gets or Sets the pdf settings from the appsettings.
    /// </summary>
    public PdfServiceConfiguration PDFService { get; set; }

    /// <summary>
    /// Gets or Sets the port settings from the appsettings.
    /// </summary>
    public PortConfig Ports { get; set; }

    /// <summary>
    /// Gets or sets the port configuration for the metric endpoint.
    /// </summary>
    public ushort MetricPort { get; set; } = 9090;

    /// <summary>
    /// Gets or Sets the razor light settings from the appsettings.
    /// </summary>
    public RazorLightConfiguration RazorLight { get; set; }

    /// <summary>
    /// Gets or Sets the secure connect settings from the appsettings.
    /// </summary>
    public SecureConnectConfiguration SecureConnect { get; set; }

    /// <summary>
    /// Gets or sets the secure connect api options.
    /// </summary>
    public SecureConnectApiOptions SecureConnectApi { get; set; } = new();

    /// <summary>
    /// Gets or Sets the supported locales from the appsettings.
    /// </summary>
    public List<string> SupportedLocales { get; set; }

    /// <summary>
    /// Gets or Sets the eCH configuration from the appsettings.
    /// </summary>
    public EchConfig Ech { get; set; }

    /// <summary>
    /// Gets or sets the auth store configuration.
    /// </summary>
    public AuthStoreConfig AuthStore { get; set; } = new();

    /// <summary>
    /// Gets or sets the malware scanner configuration.
    /// </summary>
    public MalwareScannerConfig MalwareScanner { get; set; } = new();
}
