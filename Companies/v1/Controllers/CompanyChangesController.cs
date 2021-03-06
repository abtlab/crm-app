using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Companies.Services;
using Crm.Apps.Companies.V1.Requests;
using Crm.Apps.Companies.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Companies.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireCompaniesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Companies/Changes/v1")]
    public class CompanyChangesController : AllowingCheckControllerBase
    {
        private readonly ICompaniesService _companiesService;
        private readonly ICompanyChangesService _companyChangesService;

        public CompanyChangesController(
            IUserContext userContext,
            ICompaniesService companiesService,
            ICompanyChangesService companyChangesService)
            : base(userContext)
        {
            _companyChangesService = companyChangesService;
            _companiesService = companiesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<CompanyChangeGetPagedListResponse>> GetPagedList(
            CompanyChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var company = await _companiesService.GetAsync(request.CompanyId, false, ct);
            var response = await _companyChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Companies, company.AccountId);
        }
    }
}
