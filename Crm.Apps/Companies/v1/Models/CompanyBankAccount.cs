﻿using System;
using Newtonsoft.Json;

namespace Crm.Apps.Companies.v1.Models
{
    public class CompanyBankAccount
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }

        public string Number { get; set; }

        public string BankNumber { get; set; }

        public string BankCorrespondentNumber { get; set; }

        public string BankName { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}