using System;

namespace Crm.Apps.Activities.v1.RequestParameters
{
    public class ActivityStatusGetPagedListRequestParameter
    {
        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public bool? IsFinish { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public DateTime? MinModifyDate { get; set; }

        public DateTime? MaxModifyDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; } = "CreateDateTime";

        public string OrderBy { get; set; } = "desc";
    }
}