// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess;
using Eawv.Service.Models;
using Eawv.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eawv.Service.Controllers;

[Route("api/settings")]
[ApiController]
[Authorize(Roles = Role.All)]
public class SettingController : ControllerBase
{
    private static readonly HashSet<string> AllowedLogoMimeTypes =
    [
        "image/jpeg",
        "image/png",
    ];

    private readonly SettingRepository _settingRepo;
    private readonly IMapper _mapper;
    private readonly FileValidationService _fileValidationService;

    public SettingController(
        SettingRepository settingRepository,
        IMapper mapper,
        FileValidationService fileValidationService)
    {
        _settingRepo = settingRepository;
        _mapper = mapper;
        _fileValidationService = fileValidationService;
    }

    [HttpGet]
    public async Task<SettingModel> GetSetting()
    {
        var setting = await _settingRepo.GetSetting();
        return _mapper.Map<SettingModel>(setting);
    }

    [HttpPatch]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<SettingModel> UpdateSetting([FromBody] ModifySettingModel settingModel)
    {
        var setting = await _settingRepo.GetSetting();

        // automapper cant ignore primitive nullables
        if (!settingModel.ShowBallotPaperInfos.HasValue)
        {
            settingModel.ShowBallotPaperInfos = setting.ShowBallotPaperInfos;
        }

        await _fileValidationService.ValidateFile(settingModel.TenantLogo, AllowedLogoMimeTypes, Request.HttpContext.RequestAborted);

        _mapper.Map(settingModel, setting);

        var updatedSetting = await _settingRepo.CreateOrUpdate(setting);
        return _mapper.Map<SettingModel>(updatedSetting);
    }
}
