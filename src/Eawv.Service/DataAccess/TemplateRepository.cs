// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Eawv.Service.Models.TemplateServiceModels;
using Eawv.Service.Services;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess;

public class TemplateRepository
{
    private readonly EawvContext _context;
    private readonly ITenantService _tenantService;

    public TemplateRepository(EawvContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    public async Task<TemplateEntity> GetBestMatching(TemplateType type)
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        return await _context.Templates
                   .Where(t => t.Type == type && (t.TenantId == null || t.TenantId == tenantId))
                   .OrderBy(t => t.TenantId == tenantId ? 0 : 1)
                   .FirstOrDefaultAsync() ?? throw new EntityNotFoundException(type.ToString());
    }
}
