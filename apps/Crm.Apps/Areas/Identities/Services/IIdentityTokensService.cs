﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Identities.Models;

namespace Crm.Apps.Areas.Identities.Services
{
    public interface IIdentityTokensService
    {
        Task<IdentityToken> GetAsync(Guid identityId, string value, CancellationToken ct);

        Task<Guid> CreateAsync(IdentityToken token, CancellationToken ct);

        Task SetIsUsedAsync(Guid id, CancellationToken ct);
    }
}