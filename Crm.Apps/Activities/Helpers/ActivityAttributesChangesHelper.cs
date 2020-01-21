﻿using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Activities.v1.Models;

namespace Crm.Apps.Activities.Helpers
{
    public static class ActivityAttributesChangesHelper
    {
        public static ActivityAttributeChange WithCreateLog(this ActivityAttribute attribute, Guid userId,
            Action<ActivityAttribute> action)
        {
            action(attribute);

            return new ActivityAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };
        }

        public static ActivityAttributeChange WithUpdateLog(this ActivityAttribute attribute, Guid userId,
            Action<ActivityAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            return new ActivityAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = attribute.ToJsonString()
            };
        }
    }
}