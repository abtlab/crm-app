using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Contacts.Clients;
using Crm.Clients.Contacts.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Contacts
{
    public class ContactChangesTests
    {
        private readonly ICreate _create;
        private readonly IContactsClient _contactsClient;
        private readonly IContactChangesClient _contactChangesClient;

        public ContactChangesTests(ICreate create, IContactsClient contactsClient,
            IContactChangesClient contactChangesClient)
        {
            _create = create;
            _contactsClient = contactsClient;
            _contactChangesClient = contactChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var leadSource = await _create.LeadSource.WithAccountId(account.Id).BuildAsync();
            var lead = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                ;
            var contact = await _create.Contact.WithAccountId(account.Id).WithLeadId(lead.Id).BuildAsync();
            contact.IsDeleted = true;
            await _contactsClient.UpdateAsync(contact);

            var changes = await _contactChangesClient
                .GetPagedListAsync(contactId: contact.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.ContactId == contact.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<Contact>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<Contact>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<Contact>().IsDeleted);
        }
    }
}