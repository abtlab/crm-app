﻿using System.Threading;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Accounts.Controllers
{
    [ApiController]
    [Route("Api/Accounts")]
    public class AccountsDefaultController : ControllerBase
    {
        [HttpGet("")]
        [RequireAny(Permission.System, Permission.Development)]
        public ActionResult Status(CancellationToken ct = default)
        {
            return Ok();
        }
    }
}