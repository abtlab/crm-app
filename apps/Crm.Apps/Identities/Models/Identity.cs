﻿using System;
using Newtonsoft.Json;

namespace Crm.Apps.Identities.Models
{
    public class Identity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public IdentityType Type { get; set; }

        public string Key { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsVerified { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}