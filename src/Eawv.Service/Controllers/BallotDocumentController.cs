// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Eawv.Service.Models;
using Eawv.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eawv.Service.Controllers;

[Route("api/elections/{electionId:Guid}/documents")]
[ApiController]
[Authorize(Roles = Role.All)]
public class BallotDocumentController : ControllerBase
{
    private static readonly HashSet<string> AllowedDocumentsMimeTypes =
    [
        "application/pdf",
    ];

    private readonly BallotDocumentRepository _documentRepository;
    private readonly IMapper _mapper;
    private readonly ITenantService _tenantService;
    private readonly ElectionRepository _electionRepository;
    private readonly FileValidationService _fileValidationService;

    public BallotDocumentController(
        BallotDocumentRepository documentRepository,
        IMapper mapper,
        ITenantService tenantService,
        ElectionRepository electionRepository,
        FileValidationService fileValidationService)
    {
        _documentRepository = documentRepository;
        _mapper = mapper;
        _tenantService = tenantService;
        _electionRepository = electionRepository;
        _fileValidationService = fileValidationService;
    }

    [HttpGet("{id:Guid}")]
    public async Task<BallotDocumentModel> GetDocument(Guid id)
    {
        var document = await _documentRepository.Get(id);
        return _mapper.Map<BallotDocumentModel>(document);
    }

    [HttpPost]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task<EmptyBallotDocumentModel> CreateDocument(Guid electionId, [FromBody] ModifyBallotDocumentModel documentModel)
    {
        var document = _mapper.Map<BallotDocument>(documentModel);
        document.ElectionId = electionId;

        await AssertAccessToElection(electionId);
        await _fileValidationService.ValidateFile(documentModel.Name, documentModel.Document, AllowedDocumentsMimeTypes, Request.HttpContext.RequestAborted);

        return _mapper.Map<EmptyBallotDocumentModel>(await _documentRepository.Create(document));
    }

    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = Role.Wahlverwalter)]
    public async Task DeleteDocument(Guid id)
    {
        var document = await _documentRepository.Get(id);
        await AssertAccessToElection(document.ElectionId);
        await _documentRepository.Delete(id);
    }

    private async Task AssertAccessToElection(Guid electionId)
    {
        var election = await _electionRepository.GetSimpleElection(electionId);
        var relevantTenantId = await _tenantService.GetParentOrCurrentTenantId();
        if (relevantTenantId != election.TenantId)
        {
            throw new ForbiddenException();
        }
    }
}
