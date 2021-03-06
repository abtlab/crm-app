﻿using System;
using System.Text.Json.Serialization;

namespace Crm.Apps.Leads.Models
{
    public class LeadAttributeLink
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid LeadId { get; set; }

        public Guid LeadAttributeId { get; set; }

        public string Value { get; set; }
    }
}
