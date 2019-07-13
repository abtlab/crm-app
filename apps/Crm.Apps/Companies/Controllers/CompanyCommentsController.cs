using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Parameters;
using Crm.Apps.Companies.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Companies.Controllers
{
    [ApiController]
    [Route("Api/Companies/Comments")]
    public class CompanyCommentsController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ICompaniesService _companiesService;
        private readonly ICompanyCommentsService _companyCommentsService;

        public CompanyCommentsController(IUserContext userContext, ICompaniesService companiesService,
            ICompanyCommentsService companyCommentsService)
        {
            _userContext = userContext;
            _companiesService = companiesService;
            _companyCommentsService = companyCommentsService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.SalesManagement)]
        public async Task<ActionResult<List<CompanyComment>>> GetPagedList(
            CompanyCommentGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var company = await _companiesService.GetAsync(parameter.CompanyId, ct);
            var comments = await _companyCommentsService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(comments, new[] {company.AccountId});
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.AccountOwning,
            Permission.SalesManagement)]
        public async Task<ActionResult> Create(CompanyComment comment, CancellationToken ct = default)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            var company = await _companiesService.GetAsync(comment.CompanyId, ct);

            return await ActionIfAllowed(
                () => _companyCommentsService.CreateAsync(_userContext.UserId, comment, ct)
                , new[] {company.AccountId});
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning, Permission.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                await action();

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning, Permission.ProductsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.ProductsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}