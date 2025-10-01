// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Eawv.Service.Configuration;
using Microsoft.Extensions.Logging;
using Voting.Lib.Iam.Services.ApiClient.Permission;

namespace Eawv.Service.Services;

public class ApplicationService
{
    private readonly ISecureConnectPermissionServiceClient _permissionClient;
    private readonly CacheService _cache;
    private readonly SecureConnectConfiguration _config;
    private readonly ILogger<ApplicationService> _logger;

    public ApplicationService(
        ISecureConnectPermissionServiceClient permissionClient,
        CacheService cache,
        SecureConnectConfiguration config,
        ILogger<ApplicationService> logger)
    {
        _permissionClient = permissionClient;
        _cache = cache;
        _config = config;
        _logger = logger;
    }

    public async Task<V1Application> GetEawvApplication()
    {
        return await GetApplication(_config.AppShortcut);
    }

    public async Task<V1Application> GetApplication(string shortcut)
    {
        return await _cache.GetOrCreate(shortcut, async () =>
        {
            _logger.LogDebug("No cache etry found for key {CacheKey}. Getting from repository.", $"Application.{shortcut}");
            return await _permissionClient.PermissionService_GetApplicationByShortcutAsync(shortcut, false);
        });
    }
}
