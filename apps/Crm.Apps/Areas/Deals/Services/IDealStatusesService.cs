using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;

namespace Crm.Apps.Areas.Deals.Services
{
    public interface IDealStatusesService
    {
        Task<DealStatus> GetAsync(Guid id, CancellationToken ct);

        Task<List<DealStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<DealStatus>> GetPagedListAsync(DealStatusGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, DealStatus status, CancellationToken ct);

        Task UpdateAsync(Guid userId, DealStatus oldStatus, DealStatus newStatus, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}