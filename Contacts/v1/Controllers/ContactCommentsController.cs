using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.Services;
using Crm.Apps.Contacts.V1.Requests;
using Crm.Apps.Contacts.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Contacts.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireContactsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Contacts/Comments/v1")]
    public class ContactCommentsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IContactsService _contactsService;
        private readonly IContactCommentsService _contactCommentsService;

        public ContactCommentsController(
            IUserContext userContext,
            IContactsService contactsService,
            IContactCommentsService contactCommentsService)
            : base(userContext)
        {
            _userContext = userContext;
            _contactsService = contactsService;
            _contactCommentsService = contactCommentsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ContactCommentGetPagedListResponse>> GetPagedList(
            ContactCommentGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var contact = await _contactsService.GetAsync(request.ContactId, false, ct);
            var response = await _contactCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Contacts, contact.AccountId);
        }

        [HttpPut("Create")]
        public async Task<ActionResult> Create(ContactComment comment, CancellationToken ct = default)
        {
            var contact = await _contactsService.GetAsync(comment.ContactId, false, ct);

            return await ActionIfAllowed(
                () => _contactCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                Roles.Contacts,
                contact.AccountId);
        }
    }
}
