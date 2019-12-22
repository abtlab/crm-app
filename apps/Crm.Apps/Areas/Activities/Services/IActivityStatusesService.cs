using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;

namespace Crm.Apps.Areas.Activities.Services
{
    public interface IActivityStatusesService
    {
        Task<ActivityStatus> GetAsync(Guid id, CancellationToken ct);

        Task<ActivityStatus[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<ActivityStatus[]> GetPagedListAsync(ActivityStatusGetPagedListRequest request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ActivityStatusCreateRequest request, CancellationToken ct);

        Task UpdateAsync(
            Guid userId,
            ActivityStatus status,
            ActivityStatusUpdateRequest request,
            CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}