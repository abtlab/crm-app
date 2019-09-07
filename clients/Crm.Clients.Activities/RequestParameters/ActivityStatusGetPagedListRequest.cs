using System;

namespace Crm.Clients.Activities.RequestParameters
{
    public class ActivityStatusGetPagedListRequest
    {
        public Guid AccountId { get; set; }

        public string? Name { get; set; }

        public bool? IsFinish { get; set; }

        public bool? IsDeleted { get; set; }
        
        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = 10;

        public string? SortBy { get; set; } = "CreateDateTime";

        public string? OrderBy { get; set; } = "desc";
    }
}