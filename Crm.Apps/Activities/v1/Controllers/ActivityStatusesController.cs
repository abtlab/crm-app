﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Activities.Services;
using Crm.Apps.Activities.v1.Models;
using Crm.Apps.Activities.v1.RequestParameters;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.v1.Controllers
{
    [ApiController]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Activities/Statuses/v1")]
    public class ActivityStatusesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IActivityStatusesService _activityStatusesService;

        public ActivityStatusesController(IUserContext userContext, IActivityStatusesService activityStatusesService) :
            base(userContext)
        {
            _userContext = userContext;
            _activityStatusesService = activityStatusesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ActivityStatus>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var status = await _activityStatusesService.GetAsync(id, ct);
            if (status == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(status, Roles.Sales, status.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<ActivityStatus>>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var statuses = await _activityStatusesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                statuses,
                Roles.Sales,
                statuses.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ActivityStatus>>> GetPagedList(
            ActivityStatusGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var statuses = await _activityStatusesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                statuses,
                Roles.Sales,
                statuses.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(ActivityStatus status, CancellationToken ct = default)
        {
            status.AccountId = _userContext.AccountId;

            var id = await _activityStatusesService.CreateAsync(_userContext.UserId, status, ct);

            return Created("Get", id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(ActivityStatus status, CancellationToken ct = default)
        {
            var oldStatus = await _activityStatusesService.GetAsync(status.Id, ct);
            if (oldStatus == null)
            {
                return NotFound(status.Id);
            }

            return await ActionIfAllowed(
                () => _activityStatusesService.UpdateAsync(_userContext.UserId, oldStatus, status, ct),
                Roles.Sales,
                status.AccountId, oldStatus.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityStatusesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityStatusesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }
    }
}