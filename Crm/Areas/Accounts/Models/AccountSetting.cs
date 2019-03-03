﻿using System;

namespace Crm.Areas.Accounts.Models
{
    public class AccountSetting
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public AccountSettingType Type { get; set; }

        public string Value { get; set; }
    }
}