using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Contacts.v1.RequestParameters
{
    public class ContactCommentGetPagedListRequestParameter
    {
        [Required]
        public Guid ContactId { get; set; }

        public Guid? CommentatorUserId { get; set; }

        public string Value { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; } = "CreateDateTime";

        public string OrderBy { get; set; } = "desc";
    }
}