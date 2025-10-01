// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Eawv.Service.Services;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Common;

namespace Eawv.Service.DataAccess;

public class SettingRepository : BaseRepository<Setting>
{
    private readonly ITenantService _tenantService;

    public SettingRepository(EawvContext context, AuthService authService, ITenantService tenantService, IClock clock)
        : base(context, authService, clock)
    {
        _tenantService = tenantService;
    }

    public async Task<Setting> GetSetting()
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        var entity = await Context.Settings
            .SingleOrDefaultAsync(x => x.TenantId == tenantId);
        return entity ?? new Setting { TenantId = await _tenantService.GetParentOrCurrentTenantId() };
    }

    public override async Task<Setting> Get(Guid id)
    {
        var tenantId = await _tenantService.GetParentOrCurrentTenantId();
        var entity = await Context.Settings
            .SingleOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);
        return entity ?? throw new EntityNotFoundException(id);
    }

    public async Task<Setting> CreateOrUpdate(Setting setting)
    {
        if (setting.Id == Guid.Empty)
        {
            return await Create(setting);
        }

        setting.ModifiedDate = Clock.UtcNow;
        setting.ModifiedBy = AuthService.GetUserId();

        await Save();
        return setting;
    }
}
