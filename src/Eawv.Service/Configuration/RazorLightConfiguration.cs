// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.Configuration;

/// <summary>
/// Configuration model for the 'RazorLight' section in the appsettings.
/// </summary>
public class RazorLightConfiguration
{
    /// <summary>
    /// Gets or sets the root path of the razor templates which should be relatively to the app root.
    /// Will be used for the 'https://github.com/toddams/RazorLight#file-source'.
    /// </summary>
    public string TemplatesRootPath { get; set; }
}
