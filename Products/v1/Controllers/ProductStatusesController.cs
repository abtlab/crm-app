﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.v1.Models;
using Crm.Apps.Products.v1.RequestParameters;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.v1.Controllers
{
    [ApiController]
    [RequireProductsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Products/Statuses/v1")]
    public class ProductStatusesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IProductStatusesService _productStatusesService;

        public ProductStatusesController(IUserContext userContext, IProductStatusesService productStatusesService)
            : base(userContext)
        {
            _userContext = userContext;
            _productStatusesService = productStatusesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ProductStatus>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var status = await _productStatusesService.GetAsync(id, ct);
            if (status == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(status, Roles.Products, status.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<ProductStatus>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var statuses = await _productStatusesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                statuses,
                Roles.Products,
                statuses.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ProductStatus>>> GetPagedList(
            ProductStatusGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var statuses = await _productStatusesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                statuses,
                Roles.Products,
                statuses.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(ProductStatus status, CancellationToken ct = default)
        {
            status.AccountId = _userContext.AccountId;

            var id = await _productStatusesService.CreateAsync(_userContext.UserId, status, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(ProductStatus status, CancellationToken ct = default)
        {
            var oldStatus = await _productStatusesService.GetAsync(status.Id, ct);
            if (oldStatus == null)
            {
                return NotFound(status.Id);
            }

            return await ActionIfAllowed(
                () => _productStatusesService.UpdateAsync(_userContext.UserId, oldStatus, status, ct),
                Roles.Products,
                status.AccountId, oldStatus.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _productStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productStatusesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Products,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _productStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productStatusesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Products,
                attributes.Select(x => x.AccountId));
        }
    }
}