// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Configuration;
using Eawv.Service.Exceptions;
using Eawv.Service.Models.NotificationServiceModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Voting.Lib.Iam.Services.ApiClient.Permission;

namespace Eawv.Service.Services;

public class TenantService : ITenantService
{
    private const string PartiesCacheKeySuffix = ".parties";
    private readonly AuthService _authService;
    private readonly CacheService _cache;
    private readonly SecureConnectConfiguration _config;
    private readonly ApplicationService _applicationService;
    private readonly NotificationServiceConfiguration _notificationConfig;
    private readonly ISecureConnectPermissionServiceClient _permissionClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TenantService> _logger;

    public TenantService(
        AuthService authService,
        CacheService cache,
        SecureConnectConfiguration config,
        ApplicationService applicationService,
        NotificationServiceConfiguration notificationConfig,
        ISecureConnectPermissionServiceClient permissionClient,
        IServiceProvider serviceProvider,
        ILogger<TenantService> logger)
    {
        _authService = authService;
        _cache = cache;
        _config = config;
        _applicationService = applicationService;
        _notificationConfig = notificationConfig;
        _permissionClient = permissionClient;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<V1Tenant> Get(string tenantId) =>
        await _cache.GetOrCreate(tenantId, async ()
            => await _permissionClient.PermissionService_GetTenantByIdAsync(tenantId, false));

    public async Task AssertChildParent(string childId, string parentId)
    {
        if (parentId == null)
        {
            throw new PartyTenantDoesNotMatchParentException(childId, parentId);
        }

        var childsParentId = await GetParentTenantId(childId);
        if (childsParentId != parentId)
        {
            throw new PartyTenantDoesNotMatchParentException(childId, parentId);
        }
    }

    public async Task<string> GetParentOrCurrentTenantId()
    {
        // In theory, a Wahlverwalter should never have a parent tenant, as he must be a Wahlverwalter on the parent tenant itself.
        // But we make sure that we can handle rare edge cases (misconfiguration?) where a Wahlverwalter has a parent tenant
        // by ignoring it explicitly.
        var tenantId = _authService.GetTenantId();
        if (_authService.IsWahlverwalter)
        {
            return tenantId;
        }

        // Users should always have a parent tenant in theory, so GetParentTenantId() should return it.
        // We still support the edge case / misconfiguration where a parent tenant has not been set (yet).
        return await GetParentTenantId(tenantId) ?? tenantId;
    }

    public async Task<string> GetParentTenantId(string tenantId)
    {
        return GetParentTenantId(await Get(tenantId));
    }

    public string GetParentTenantId(V1Tenant tenant)
    {
        return GetParentTenantIdFromLabels(tenant.TenantExt.Labels) ??
               GetParentTenantIdFromLabels(tenant.Labels);
    }

    public async Task<IEnumerable<V1Tenant>> GetParties()
    {
        var tenantId = _authService.GetTenantId();
        return await _cache.GetOrCreate<IEnumerable<V1Tenant>, V1Tenant>(tenantId + PartiesCacheKeySuffix, async () =>
        {
            _logger.LogDebug("No cache entry found for key {CacheKey}. Getting from repository.", $"Tenant.{tenantId + PartiesCacheKeySuffix}");
            var application = await _applicationService.GetEawvApplication();
            var tenants = await _permissionClient.PermissionService_GetTenantsByApplicationAsync(application.Id);
            return tenants.Tenants.Where(t => GetParentTenantId(t) == tenantId);
        });
    }

    public async Task CreateParty(string name)
    {
        const string subjectTemplate = @"Neuer Mandant für VO Wahlvorschlag | Mandant ""{0}"" für Oberbehörde ""{1}""";
        const string contentTemplate = """
            <p>In VOTING Wahlvorschlag wurde von der Oberbehörde <strong>{1}</strong> ein neuer Mandant (Partei) <strong>{0}</strong> beantragt.</p>
            <p><strong>Konfiguration Mandant</strong></p>
            <ul>
            <li>Name: {0}</li>
            <li>Berechtigung: VOTING Wahlvorschlag</li>
            <li>Rolle: Benutzer</li>
            <li>Label Key: {2}</li>
            <li>Label Wert: {3}</li>
            </ul>
            <p><strong>Zuweisung Berechtigung für Service User</strong></p>
            <ul>
            <li>Service Benutzername: {5}</li>
            <li>Mandant: {0}</li>
            <li>Applikation: SECURE Access</li>
            <li>Rolle: TenantManager</li>
            </ul>
            <p>Nach der erfolgreichen Erstellung des neuen Mandanten wird dieser in den Stammdaten der Oberbehörde angezeigt und kann für die Berechtigung von Benutzern verwendet werden.</p>
            <p>Antragssteller Subject Id: {4}</p>
            <p><em>Diese Nachricht wurde automatisch generiert.</em></p>
            """;

        await HandlePartyNotification(
            () => Task.FromResult(name),
            subjectTemplate,
            contentTemplate);
    }

    public async Task RemoveParty(string id)
    {
        var partyIds = (await GetParties()).Select(p => p.Id).ToList();
        if (!partyIds.Contains(id))
        {
            throw new ForbiddenException(id);
        }

        const string subjectTemplate = @"Entfernen von Mandant für VO Wahlvorschlag | Mandant ""{0}"" für Oberbehörde ""{1}""";
        const string contentTemplate = """
            <p>In VOTING Wahlvorschlag wurde von der Oberbehörde <strong>{1}</strong> die Löschung des Mandanten (Partei) <strong>{0}</strong> beantragt.
            Die Berechtigungen und das Label müssen entfernt werden. Wird der Mandant nicht mehr benötigt, muss eine Löschung des Mandanten durchgeführt werden.</p>
            <p><strong>Konfiguration Mandant</strong></p>
            <ul>
            <li>Name: {0}</li>
            <li>Berechtigung: VOTING Wahlvorschlag</li>
            <li>Rolle: Benutzer</li>
            <li>Label Key: {2}</li>
            <li>Label Wert: {3}</li>
            </ul>
            <p><strong>Entfernung Berechtigung für Service User</strong></p>
            <ul>
            <li>Service Benutzername: {5}</li>
            <li>Mandant: {0}</li>
            <li>Applikation: SECURE Access</li>
            <li>Rolle: TenantManager</li>
            </ul>
            <p>Nach erfolgreicher Löschung des Mandanten wird dieser aus den Stammdaten der Oberbehörde entfernt und steht nicht mehr für die Benutzerberechtigung zur Verfügung.</p>
            <p>Antragssteller Subject Id: {4}</p>
            <p><em>Diese Nachricht wurde automatisch generiert.</em></p>
            """;

        await HandlePartyNotification(
            async () => (await Get(id)).Name,
            subjectTemplate,
            contentTemplate);
    }

    private async Task HandlePartyNotification(
        Func<Task<string>> fetchPartyNameFunc,
        string subjectTemplate,
        string contentTemplate)
    {
        var notificationService = _serviceProvider.GetRequiredService<INotificationService>();
        var currentTenant = await Get(_authService.GetTenantId());
        var sanitizedCurrentTenantId = HtmlEncoder.Default.Encode(currentTenant.Id);
        var sanitizedCurrentTenantName = HtmlEncoder.Default.Encode(currentTenant.Name);
        var sanitizedParentLabelName = HtmlEncoder.Default.Encode(_config.ParentLabelName);
        var sanitizedApplicantId = HtmlEncoder.Default.Encode(_authService.GetUserId());
        var partyName = await fetchPartyNameFunc();
        var sanitizedPartyName = HtmlEncoder.Default.Encode(partyName);
        var sanitizedServiceUserName = HtmlEncoder.Default.Encode(_config.ServiceAccount);

        var subject = string.Format(subjectTemplate, sanitizedPartyName, sanitizedCurrentTenantName);
        var content = string.Format(
            contentTemplate,
            sanitizedPartyName,
            sanitizedCurrentTenantName,
            sanitizedParentLabelName,
            sanitizedCurrentTenantId,
            sanitizedApplicantId,
            sanitizedServiceUserName);

        await notificationService.SendEmailAsync(new SendEmailRequestModel
        {
            Sender = new() { DisplayName = _notificationConfig.SenderEmail },
            Recipients = [new() { EmailAddress = _notificationConfig.SupportEmail }],
            Message = new()
            {
                Subject = new() { Raw = subject },
                Content = new() { Raw = content },
            },
        });
    }

    private string GetParentTenantIdFromLabels(IEnumerable<CommonapiLabel> labels)
    {
        return labels?.FirstOrDefault(l => l.Key == _config.ParentLabelName)?.Value;
    }
}
