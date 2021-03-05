﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Flags.Models;

namespace Crm.Apps.Flags.Services
{
    public interface IUserFlagsService
    {
        Task<bool> IsSetAsync(Guid userId, UserFlagType type, CancellationToken ct);

        Task<IEnumerable<UserFlagType>> GetNotSetListAsync(Guid userId, CancellationToken ct);

        Task SetAsync(Guid userId, UserFlagType type, CancellationToken ct);
    }
}
