using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;

namespace Crm.Apps.Areas.Activities.Services
{
    public interface IActivityAttributesService
    {
        Task<ActivityAttribute> GetAsync(Guid id, CancellationToken ct);

        Task<ActivityAttribute[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<ActivityAttribute[]> GetPagedListAsync(ActivityAttributeGetPagedListRequest request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ActivityAttributeCreateRequest request, CancellationToken ct);

        Task UpdateAsync(
            Guid userId,
            ActivityAttribute attribute,
            ActivityAttributeUpdateRequest request,
            CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}