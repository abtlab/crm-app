using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.V1.Requests;
using Crm.Apps.Products.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireProductsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Products/Changes/v1")]
    public class ProductChangesController : AllowingCheckControllerBase
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
        public async Task<ActionResult<ProductChangeGetPagedListResponse>> GetPagedList(
            ProductChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var product = await _productsService.GetAsync(request.ProductId, false, ct);
            var response = await _productChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Products, product.AccountId);
        }
    }
}
