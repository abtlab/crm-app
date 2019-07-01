using System;
using System.Threading.Tasks;
using Crm.Common.Types;

namespace Crm.Apps.Tests.Builders.Leads
{
    public interface ILeadAttributeBuilder
    {
        LeadAttributeBuilder WithAccountId(Guid accountId);

        LeadAttributeBuilder WithType(AttributeType type);

        LeadAttributeBuilder WithKey(string key);

        Task<Clients.Leads.Models.LeadAttribute> BuildAsync();
    }
}