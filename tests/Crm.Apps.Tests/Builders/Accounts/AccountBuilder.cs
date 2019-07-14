using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Clients;
using Crm.Clients.Accounts.Models;

namespace Crm.Apps.Tests.Builders.Accounts
{
    public class AccountBuilder : IAccountBuilder
    {
        private readonly IAccountsClient _accountsClient;
        private readonly Account _account;

        public AccountBuilder(
            IAccountsClient accountsClient)
        {
            _accountsClient = accountsClient;
            _account = new Account(AccountType.MlmSystem);
        }

        public AccountBuilder AsLocked()
        {
            _account.IsLocked = true;

            return this;
        }

        public AccountBuilder AsDeleted()
        {
            _account.IsDeleted = true;

            return this;
        }

        public AccountBuilder WithSetting(
            AccountSettingType type,
            string value)
        {
            if (_account.Settings == null)
            {
                _account.Settings = new List<AccountSetting>();
            }

            _account.Settings.Add(new AccountSetting(type, value));

            return this;
        }

        public async Task<Account> BuildAsync()
        {
            var createdId = await _accountsClient.CreateAsync(_account);

            return await _accountsClient.GetAsync(createdId);
        }
    }
}