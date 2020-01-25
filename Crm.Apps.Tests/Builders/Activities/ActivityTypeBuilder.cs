using System;
using System.Threading.Tasks;
using Crm.Apps.Tests.Extensions;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.v1.Clients.Activities.Clients;
using Crm.Apps.v1.Clients.Activities.Models;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityTypeBuilder : IActivityTypeBuilder
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly IActivityTypesClient _activityTypesClient;
        private readonly ActivityType _type;

        public ActivityTypeBuilder(IAccessTokenGetter accessTokenGetter, IActivityTypesClient activityTypesClient)
        {
            _activityTypesClient = activityTypesClient;
            _accessTokenGetter = accessTokenGetter;
            _type = new ActivityType
            {
                AccountId = Guid.Empty,
                Name = "Test".WithGuid(),
                IsDeleted = false
            };
        }

        public ActivityTypeBuilder WithName(string name)
        {
            _type.Name = name;

            return this;
        }

        public ActivityTypeBuilder AsDeleted()
        {
            _type.IsDeleted = true;

            return this;
        }

        public async Task<ActivityType> BuildAsync()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var id = await _activityTypesClient.CreateAsync(accessToken, _type);

            return await _activityTypesClient.GetAsync(accessToken, id);
        }
    }
}