using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Helpers;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;
using Crm.Apps.Areas.Activities.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Activities.Services
{
    public class ActivityTypesService : IActivityTypesService
    {
        private readonly ActivitiesStorage _activitiesStorage;

        public ActivityTypesService(ActivitiesStorage activitiesStorage)
        {
            _activitiesStorage = activitiesStorage;
        }

        public Task<ActivityType> GetAsync(Guid id, CancellationToken ct)
        {
            return _activitiesStorage.ActivityTypes.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<ActivityType[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _activitiesStorage.ActivityTypes
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync(ct);
        }

        public Task<ActivityType[]> GetPagedListAsync(ActivityTypeGetPagedListRequest request, CancellationToken ct)
        {
            return _activitiesStorage.ActivityTypes
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, ActivityTypeCreateRequest request, CancellationToken ct)
        {
            var type = new ActivityType();
            var change = type.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = request.AccountId;
                x.Name = request.Name;
                x.IsDeleted = request.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _activitiesStorage.AddAsync(type, ct);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            ActivityType type,
            ActivityTypeUpdateRequest request,
            CancellationToken ct)
        {
            var change = type.WithUpdateLog(userId, x =>
            {
                x.Name = request.Name;
                x.IsDeleted = request.IsDeleted;
            });

            _activitiesStorage.Update(type);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityTypeChange>();

            await _activitiesStorage.ActivityTypes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityTypeChange>();

            await _activitiesStorage.ActivityTypes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }
    }
}