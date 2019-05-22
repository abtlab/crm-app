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
    [Route("Api/Users/Changes")]
    public class UserChangesController : ControllerBase
    {
        private readonly IUserChangesService _userChangesService;

        public UserChangesController(IUserChangesService userChangesService)
        {
            _userChangesService = userChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<UserChange>>> GetPagedList(UserChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _userChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}