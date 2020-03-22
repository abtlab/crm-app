﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Services;
using Crm.Apps.Deals.v1.Requests;
using Crm.Apps.Deals.v1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.v1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Deals/Types/v1")]
    public class DealTypesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IDealTypesService _dealTypesService;

        public DealTypesController(IUserContext userContext, IDealTypesService dealTypesService)
            : base(userContext)
        {
            _userContext = userContext;
            _dealTypesService = dealTypesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<DealType>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var type = await _dealTypesService.GetAsync(id, ct);
            if (type == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(type, Roles.Sales, type.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<DealType>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var types = await _dealTypesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                types,
                Roles.Sales,
                types.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<DealTypeGetPagedListResponse>> GetPagedList(
            DealTypeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _dealTypesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Sales,
                response.Types.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(DealType type, CancellationToken ct = default)
        {
            type.AccountId = _userContext.AccountId;

            var id = await _dealTypesService.CreateAsync(_userContext.UserId, type, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(DealType type, CancellationToken ct = default)
        {
            var oldType = await _dealTypesService.GetAsync(type.Id, ct);
            if (oldType == null)
            {
                return NotFound(type.Id);
            }

            return await ActionIfAllowed(
                () => _dealTypesService.UpdateAsync(_userContext.UserId, oldType, type, ct),
                Roles.Sales,
                type.AccountId, oldType.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _dealTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealTypesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _dealTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealTypesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }
    }
}