using System;
using System.Collections.Generic;

namespace Crm.Apps.Contacts.v1.Models
{
    public class Contact
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public Guid LeadId { get; set; }

        public Guid? CompanyId { get; set; }

        public Guid CreateUserId { get; set; }

        public Guid? ResponsibleUserId { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string TaxNumber { get; set; }

        public string Post { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string Apartment { get; set; }

        public DateTime? BirthDate { get; set; }

        public byte[] Photo { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }

        public List<ContactBankAccount> BankAccounts { get; set; }

        public List<ContactAttributeLink> AttributeLinks { get; set; }
    }
}