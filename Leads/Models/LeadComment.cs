﻿using System;

namespace Crm.Apps.Leads.Models
{
    public class LeadComment
    {
        public Guid Id { get; set; }

        public Guid LeadId { get; set; }

        public Guid CommentatorUserId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}