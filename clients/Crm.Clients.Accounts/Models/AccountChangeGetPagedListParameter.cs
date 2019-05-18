using System;

namespace Crm.Clients.Accounts.Models
{
    public class AccountChangeGetPagedListParameter
    {
        public Guid? ChangerUserId { get; set; }

        public Guid? AccountId { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}