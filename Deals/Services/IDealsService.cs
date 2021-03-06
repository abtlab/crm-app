﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.V1.Requests;
using Crm.Apps.Deals.V1.Responses;

namespace Crm.Apps.Deals.Services
{
    public interface IDealsService
    {
        Task<Deal> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<Deal>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<DealGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            DealGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Deal deal, CancellationToken ct);

        Task UpdateAsync(Guid userId, Deal oldDeal, Deal newDeal, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
