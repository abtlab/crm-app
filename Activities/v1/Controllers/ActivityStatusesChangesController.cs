using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Activities.Services;
using Crm.Apps.Activities.V1.Requests;
using Crm.Apps.Activities.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireActivitiesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Activities/Statuses/Changes/v1")]
    public class ActivityStatusesChangesController : AllowingCheckControllerBase
    {
        private readonly IActivityStatusesService _activityStatusesService;
        private readonly IActivityStatusChangesService _activityStatusChangesService;

        public ActivityStatusesChangesController(
            IUserContext userContext,
            IActivityStatusesService activityStatusesService,
            IActivityStatusChangesService activityStatusChangesService)
            : base(userContext)
        {
            _activityStatusesService = activityStatusesService;
            _activityStatusChangesService = activityStatusChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityStatusChangeGetPagedListResponse>> GetPagedList(
            ActivityStatusChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var status = await _activityStatusesService.GetAsync(request.StatusId, false, ct);
            var response = await _activityStatusChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Activities, status.AccountId);
        }
    }
}
