﻿using System;

namespace Crm.Apps.Contacts.Models
{
    public class ContactComment
    {
        public Guid Id { get; set; }

        public Guid ContactId { get; set; }

        public Guid CommentatorUserId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}