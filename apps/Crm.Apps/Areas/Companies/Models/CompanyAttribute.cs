﻿using System;
using Crm.Common.Types;

namespace Crm.Apps.Areas.Companies.Models
{
    public class CompanyAttribute
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public AttributeType Type { get; set; }

        public string Key { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}