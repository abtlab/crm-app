﻿using System;

namespace Crm.Apps.Activities.Models
{
    public class ActivityTypeChange
    {
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid TypeId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}