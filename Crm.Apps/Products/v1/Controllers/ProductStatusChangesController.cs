using System.Collections.Generic;
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
    [Route("api/v1/Products/Statuses/Changes")]
    public class ProductStatusesChangesController : AllowingCheckControllerBase
    {
        private readonly IProductStatusesService _productStatusesService;
        private readonly IProductStatusChangesService _userStatusChangesService;

        public ProductStatusesChangesController(
            IUserContext userContext,
            IProductStatusesService productStatusesService,
            IProductStatusChangesService userStatusChangesService)
            : base(userContext)

        {
            _productStatusesService = productStatusesService;
            _userStatusChangesService = userStatusChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ProductStatusChange>>> GetPagedList(
            ProductStatusChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var status = await _productStatusesService.GetAsync(request.StatusId, ct);
            var changes = await _userStatusChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Roles.Products, status.AccountId);
        }
    }
}