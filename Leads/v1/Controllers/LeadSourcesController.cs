﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Services;
using Crm.Apps.Leads.V1.Requests;
using Crm.Apps.Leads.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireLeadsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Leads/Sources/v1")]
    public class LeadSourcesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ILeadSourcesService _leadSourcesService;

        public LeadSourcesController(IUserContext userContext, ILeadSourcesService leadSourcesService)
            : base(userContext)
        {
            _userContext = userContext;
            _leadSourcesService = leadSourcesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<LeadSource>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var source = await _leadSourcesService.GetAsync(id, false, ct);
            if (source == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(source, Roles.Leads, source.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<LeadSource>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var sources = await _leadSourcesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                sources,
                Roles.Leads,
                sources.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<LeadSourceGetPagedListResponse>> GetPagedList(
            LeadSourceGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _leadSourcesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Leads,
                response.Sources.Select(x => x.AccountId));
        }

        [HttpPut("Create")]
        public async Task<ActionResult<Guid>> Create(LeadSource source, CancellationToken ct = default)
        {
            source.AccountId = _userContext.AccountId;

            var id = await _leadSourcesService.CreateAsync(_userContext.UserId, source, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(LeadSource source, CancellationToken ct = default)
        {
            var oldSource = await _leadSourcesService.GetAsync(source.Id, true, ct);
            if (oldSource == null)
            {
                return NotFound(source.Id);
            }

            return await ActionIfAllowed(
                () => _leadSourcesService.UpdateAsync(_userContext.UserId, oldSource, source, ct),
                Roles.Leads,
                oldSource.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var sources = await _leadSourcesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadSourcesService.DeleteAsync(_userContext.UserId, sources.Select(x => x.Id), ct),
                Roles.Leads,
                sources.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var sources = await _leadSourcesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadSourcesService.RestoreAsync(_userContext.UserId, sources.Select(x => x.Id), ct),
                Roles.Leads,
                sources.Select(x => x.AccountId));
        }
    }
}
