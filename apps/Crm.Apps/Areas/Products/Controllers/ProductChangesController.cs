using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.Parameters;
using Crm.Apps.Areas.Products.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Products.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.ProductsManagement)]
    [Route("Api/Products/Changes")]
    public class ProductChangesController : UserContextController
    {
        private readonly IProductsService _productsService;
        private readonly IProductChangesService _productChangesService;

        public ProductChangesController(
            IUserContext userContext,
            IProductsService productsService,
            IProductChangesService productChangesService)
            : base(userContext)
        {
            _productsService = productsService;
            _productChangesService = productChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ProductChange>>> GetPagedList(
            ProductChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var product = await _productsService.GetAsync(parameter.ProductId, ct);
            var changes = await _productChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.ProductsManagement}, product.AccountId);
        }
    }
}