using System.Collections.Generic;
using Crm.Apps.Leads.Models;

namespace Crm.Apps.Leads.V1.Responses
{
    public class LeadAttributeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<LeadAttributeChange> Changes { get; set; }
    }
}
