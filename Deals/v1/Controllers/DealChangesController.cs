using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Deals.Services;
using Crm.Apps.Deals.V1.Requests;
using Crm.Apps.Deals.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireDealsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Deals/Changes/v1")]
    public class DealChangesController : AllowingCheckControllerBase
    {
        private readonly IDealsService _dealsService;
        private readonly IDealChangesService _dealChangesService;

        public DealChangesController(
            IUserContext userContext,
            IDealsService dealsService,
            IDealChangesService dealChangesService)
            : base(userContext)
        {
            _dealChangesService = dealChangesService;
            _dealsService = dealsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<DealChangeGetPagedListResponse>> GetPagedList(
            DealChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var deal = await _dealsService.GetAsync(request.DealId, false, ct);
            var response = await _dealChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Deals, deal.AccountId);
        }
    }
}
