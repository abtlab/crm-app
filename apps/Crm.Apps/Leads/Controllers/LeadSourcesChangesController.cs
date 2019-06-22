using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Parameters;
using Crm.Apps.Leads.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.Controllers
{
    [ApiController]
    [Route("Api/Leads/Sources/Changes")]
    public class LeadSourcesChangesController : ControllerBase
    {
        private readonly ILeadSourceChangesService _userSourceChangesService;

        public LeadSourcesChangesController(ILeadSourceChangesService userSourceChangesService)
        {
            _userSourceChangesService = userSourceChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<LeadSourceChange>>> GetPagedList(
            LeadSourceChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _userSourceChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}