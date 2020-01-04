using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Companies.Clients;
using Crm.Clients.Companies.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Companies
{
    public class CompanyChangesTests
    {
        private readonly ICreate _create;
        private readonly ICompaniesClient _companiesClient;
        private readonly ICompanyChangesClient _companyChangesClient;

        public CompanyChangesTests(ICreate create, ICompaniesClient companiesClient,
            ICompanyChangesClient companyChangesClient)
        {
            _create = create;
            _companiesClient = companiesClient;
            _companyChangesClient = companyChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var leadSource = await _create.LeadSource.WithAccountId(account.Id).BuildAsync();
            var lead = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                ;
            var company = await _create.Company.WithAccountId(account.Id).WithLeadId(lead.Id).BuildAsync()
                ;
            company.IsDeleted = true;
            await _companiesClient.UpdateAsync(company);

            var changes = await _companyChangesClient
                .GetPagedListAsync(companyId: company.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.CompanyId == company.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<Company>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<Company>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<Company>().IsDeleted);
        }
    }
}