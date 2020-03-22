using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
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
    [Route("Deals/Attributes/Changes/v1")]
    public class DealAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly IDealAttributesService _dealAttributesService;
        private readonly IDealAttributeChangesService _dealAttributeChangesService;

        public DealAttributeChangesController(
            IUserContext userContext,
            IDealAttributesService dealAttributesService,
            IDealAttributeChangesService dealAttributeChangesService)
            : base(userContext)
        {
            _dealAttributeChangesService = dealAttributeChangesService;
            _dealAttributesService = dealAttributesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<DealAttributeChangeGetPagedListResponse>> GetPagedList(
            DealAttributeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var attribute = await _dealAttributesService.GetAsync(request.AttributeId, ct);
            var response = await _dealAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Sales, attribute.AccountId);
        }
    }
}