using System;

namespace Crm.Apps.Accounts.RequestParameters
{
    public class AccountChangeGetPagedListRequest
    {
        public Guid AccountId { get; set; }

        public Guid? ChangerUserId { get; set; }

        public DateTime? MinCreateDate { get; set; } = DateTime.UtcNow.AddDays(-1);

        public DateTime? MaxCreateDate { get; set; } = DateTime.UtcNow;

        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = 10;

        public string? SortBy { get; set; } = "CreateDateTime";

        public string? OrderBy { get; set; } = "desc";
    }
}