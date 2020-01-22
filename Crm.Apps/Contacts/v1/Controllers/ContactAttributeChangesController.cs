using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Companies.Services;
using Crm.Apps.Contacts.Services;
using Crm.Apps.Contacts.v1.Models;
using Crm.Apps.Contacts.v1.RequestParameters;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Contacts.v1.Controllers
{
    [ApiController]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Contacts/Attributes/Changes/v1")]
    public class ContactAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly ICompanyAttributesService _companyAttributesService;
        private readonly IContactAttributeChangesService _contactAttributeChangesService;

        public ContactAttributeChangesController(
            IUserContext userContext,
            ICompanyAttributesService companyAttributesService,
            IContactAttributeChangesService contactAttributeChangesService)
            : base(userContext)
        {
            _companyAttributesService = companyAttributesService;
            _contactAttributeChangesService = contactAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ContactAttributeChange>>> GetPagedList(
            ContactAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var attribute = await _companyAttributesService.GetAsync(request.AttributeId, ct);
            var changes = await _contactAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Roles.Sales, attribute.AccountId);
        }
    }
}