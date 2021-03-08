﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Settings.Helpers;
using Crm.Apps.Settings.Models;
using Crm.Apps.Settings.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Settings.Services
{
    public class AccountSettingsService : IAccountSettingsService
    {
        private readonly SettingsStorage _storage;

        public AccountSettingsService(SettingsStorage storage)
        {
            _storage = storage;
        }

        public Task<AccountSetting> GetAsync(Guid accountId, CancellationToken ct)
        {
            return _storage.AccountSettings
                .AsTracking()
                .SingleOrDefaultAsync(x => x.AccountId == accountId, ct);
        }

        public Task SetActivityIndustryAsync(
            Guid userId,
            Guid accountId,
            AccountSettingActivityIndustry industry,
            CancellationToken ct)
        {
            return SetAsync(userId, accountId, x => x.ActivityIndustry = industry, ct);
        }

        private async Task SetAsync(
            Guid userId,
            Guid accountId,
            Action<AccountSetting> setFunction,
            CancellationToken ct)
        {
            var setting = await _storage.AccountSettings
                .FirstOrDefaultAsync(x => x.AccountId == accountId, ct);

            if (setting == null)
            {
                setting = new AccountSetting();
                var change = setting.CreateWithLog(userId, x =>
                {
                    x.Id = Guid.NewGuid();
                    x.AccountId = accountId;
                    setFunction(x);
                    x.CreateDateTime = DateTime.UtcNow;
                });

                await _storage.AddAsync(setting, ct);
                await _storage.AddAsync(change, ct);
            }
            else
            {
                var change = setting.UpdateWithLog(userId, x =>
                {
                    setFunction(x);
                    x.ModifyDateTime = DateTime.UtcNow;
                });

                _storage.Update(setting);
                await _storage.AddAsync(change, ct);
            }

            await _storage.SaveChangesAsync(ct);
        }
    }
}
