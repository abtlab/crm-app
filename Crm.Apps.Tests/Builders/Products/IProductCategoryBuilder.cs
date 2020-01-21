using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Products.Models;

namespace Crm.Apps.Tests.Builders.Products
{
    public interface IProductCategoryBuilder
    {
        ProductCategoryBuilder WithName(string name);

        ProductCategoryBuilder IsDeleted();

        Task<ProductCategory> BuildAsync();
    }
}