﻿using System;

namespace Crm.Apps.Contacts.v1.Models
{
    public class ContactAttributeLink
    {
        public Guid Id { get; set; }

        public Guid ContactId { get; set; }

        public Guid ContactAttributeId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
        
        public DateTime? ModifyDateTime { get; set; }
    }
}