using System.Collections.Generic;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.V1.Responses
{
    public class ProductChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ProductChange> Changes { get; set; }
    }
}
