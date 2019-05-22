using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Parameters;
using Crm.Apps.Areas.Users.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
{
    [ApiController]
    [Route("Api/Users/Groups/Changes")]
    public class UserGroupChangesController : ControllerBase
    {
        private readonly IUserGroupChangesService _userGroupChangesService;

        public UserGroupChangesController(IUserGroupChangesService userGroupChangesService)
        {
            _userGroupChangesService = userGroupChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<UserGroupChange>>> GetPagedList(
            UserGroupChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _userGroupChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}