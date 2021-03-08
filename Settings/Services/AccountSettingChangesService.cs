﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Settings.Storages;
using Crm.Apps.Settings.V1.Requests;
using Crm.Apps.Settings.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Settings.Services
{
    public class AccountSettingChangesService : IAccountSettingChangesService
    {
        private readonly SettingsStorage _storage;

        public AccountSettingChangesService(SettingsStorage storage)
        {
            _storage = storage;
        }

        public async Task<AccountSettingChangeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            AccountSettingChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.AccountSettingChanges
                .AsNoTracking()
                .Where(x =>
                    x.AccountId == accountId &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new AccountSettingChangeGetPagedListResponse
            {
                TotalCount = await changes
                    .CountAsync(ct),
                Changes = await changes
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }
    }
}
