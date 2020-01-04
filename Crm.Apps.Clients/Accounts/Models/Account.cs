﻿using System;
using System.Collections.Generic;

namespace Crm.Apps.Clients.Accounts.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public AccountType Type { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public ICollection<AccountSetting>? Settings { get; set; }
    }
}