using System.Collections.Generic;
using Crm.Apps.Deals.Models;

namespace Crm.Apps.Deals.v1.Responses
{
    public class DealCommentGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<DealComment> Comments { get; set; }
    }
}