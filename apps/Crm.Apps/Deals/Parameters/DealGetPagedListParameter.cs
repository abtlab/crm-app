using System;
using System.Collections.Generic;

namespace Crm.Apps.Deals.Parameters
{
    public class DealGetPagedListParameter
    {
        public Guid? AccountId { get; set; }

        public string Name { get; set; }

        public DateTime? MinStartDateTime { get; set; }

        public DateTime? MaxStartDateTime { get; set; }

        public DateTime? MinEndDateTime { get; set; }

        public DateTime? MaxEndDateTime { get; set; }

        public decimal? MinSum { get; set; }

        public decimal? MaxSum { get; set; }

        public decimal? MinSumWithDiscount { get; set; }

        public decimal? MaxSumWithDiscount { get; set; }

        public decimal? MinFinishProbability { get; set; }

        public decimal? MaxFinishProbability { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public List<Guid> TypeIds { get; set; }

        public List<Guid> StatusIds { get; set; }

        public List<Guid> CompanyIds { get; set; }

        public List<Guid> ContactIds { get; set; }

        public List<Guid> CreateUserIds { get; set; }

        public List<Guid> ResponsibleUserIds { get; set; }

        public bool? AllAttributes { get; set; }

        public IDictionary<Guid, string> Attributes { get; set; }
        
        public List<Guid> PositionsProductIds { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}