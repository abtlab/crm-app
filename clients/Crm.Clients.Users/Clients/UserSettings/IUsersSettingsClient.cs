﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;

namespace Crm.Clients.Users.Clients.UserSettings
{
    public interface IUsersSettingsClient
    {
        Task<List<UserSettingType>> GetTypesAsync(CancellationToken ct = default);
    }
}