﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Services;
using Crm.Apps.Companies.V1.Requests;
using Crm.Apps.Companies.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Companies.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [RequireCompaniesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Companies/v1")]
    public class CompaniesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ICompaniesService _companiesService;

        public CompaniesController(IUserContext userContext, ICompaniesService companiesService)
            : base(userContext)
        {
            _userContext = userContext;
            _companiesService = companiesService;
        }

        [HttpGet("GetTypes")]
        public ActionResult<Dictionary<string, CompanyType>> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<CompanyType>();
        }

        [HttpGet("GetIndustryTypes")]
        public ActionResult<Dictionary<string, CompanyIndustryType>> GetIndustryTypes()
        {
            return EnumsExtensions.GetAsDictionary<CompanyIndustryType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Company>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var company = await _companiesService.GetAsync(id, false, ct);
            if (company == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(company, Roles.Companies, company.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<Company>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var companies = await _companiesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(companies, Roles.Companies, companies.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<CompanyGetPagedListResponse>> GetPagedList(
            CompanyGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _companiesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Companies,
                response.Companies.Select(x => x.AccountId));
        }

        [HttpPut("Create")]
        public async Task<ActionResult<Guid>> Create(Company company, CancellationToken ct = default)
        {
            company.AccountId = _userContext.AccountId;

            var id = await _companiesService.CreateAsync(_userContext.UserId, company, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(Company company, CancellationToken ct = default)
        {
            var oldCompany = await _companiesService.GetAsync(company.Id, true, ct);
            if (oldCompany == null)
            {
                return NotFound(company.Id);
            }

            return await ActionIfAllowed(
                () => _companiesService.UpdateAsync(_userContext.UserId, oldCompany, company, ct),
                Roles.Companies,
                oldCompany.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var companies = await _companiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _companiesService.DeleteAsync(_userContext.UserId, companies.Select(x => x.Id), ct),
                Roles.Companies,
                companies.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var companies = await _companiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _companiesService.RestoreAsync(_userContext.UserId, companies.Select(x => x.Id), ct),
                Roles.Companies,
                companies.Select(x => x.AccountId));
        }
    }
}
