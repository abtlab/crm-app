using System;

namespace Crm.Apps.Areas.Products.Parameters
{
    public class ProductStatusGetPagedListParameter
    {
        public Guid? AccountId { get; set; }

        public string Name { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}