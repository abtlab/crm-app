﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.RequestParameters;
using Crm.Apps.Leads.Services;
using Crm.Common.All.Types.AttributeType;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.LeadsManagement)]
    [Route("Api/Leads/Attributes")]
    public class LeadAttributesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ILeadAttributesService _leadAttributesService;

        public LeadAttributesController(IUserContext userContext, ILeadAttributesService leadAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _leadAttributesService = leadAttributesService;
        }

        [HttpGet("GetTypes")]
        public Dictionary<string, AttributeType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AttributeType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<LeadAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _leadAttributesService.GetAsync(id, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, new[] {Role.AccountOwning, Role.LeadsManagement}, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<LeadAttribute>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _leadAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                attributes,
                new[] {Role.AccountOwning, Role.LeadsManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<LeadAttribute>>> GetPagedList(
            LeadAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var attributes = await _leadAttributesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                attributes,
                new[] {Role.AccountOwning, Role.LeadsManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(LeadAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _leadAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(LeadAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _leadAttributesService.GetAsync(attribute.Id, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _leadAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                new[] {Role.AccountOwning, Role.LeadsManagement},
                attribute.AccountId, oldAttribute.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _leadAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.LeadsManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _leadAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.LeadsManagement},
                attributes.Select(x => x.AccountId));
        }
    }
}