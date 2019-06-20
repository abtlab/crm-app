using System;

namespace Crm.Apps.Products.Parameters
{
    public class ProductAttributeChangeGetPagedListParameter
    {
        public Guid? ChangerUserId { get; set; }
        
        public Guid? AttributeId { get; set; }
        
        public DateTime? MinCreateDate { get; set; }
        
        public DateTime? MaxCreateDate { get; set; }
        
        public int Offset { get; set; }
        
        public int Limit { get; set; } = 10;
        
        public string SortBy { get; set; }
        
        public string OrderBy { get; set; }
    }
}