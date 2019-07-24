using System;
using Crm.Apps.Accounts.Models;

namespace Crm.Apps.OAuth.Parameters
{
    public class OAuthClientGetPagedListParameter
    {
        public OAuthClientGetPagedListParameter(
            bool? isLocked = default,
            bool? isDeleted = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            AccountType[] types = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc")
        {
            IsLocked = isLocked;
            IsDeleted = isDeleted;
            MinCreateDate = minCreateDate;
            MaxCreateDate = maxCreateDate;
            Types = types;
            Offset = offset;
            Limit = limit;
            OrderBy = orderBy;
            SortBy = sortBy;
        }

        public bool? IsLocked { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public AccountType[] Types { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}