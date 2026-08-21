// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using AutoMapper;
using Eawv.Service.Authentication;
using Eawv.Service.Data.Countries;
using Eawv.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eawv.Service.Controllers;

[Route("api/countries")]
[ApiController]
[Authorize(Roles = Role.All)]
public class CountryController
{
    private readonly IMapper _mapper;

    public CountryController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public IEnumerable<CountryModel> GetCountries()
    {
        var countries = CountryProvider.GetAll();
        return _mapper.Map<IEnumerable<CountryModel>>(countries);
    }
}
