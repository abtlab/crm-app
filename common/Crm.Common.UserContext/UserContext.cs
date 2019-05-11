﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Crm.Common.UserContext
{
    public class UserContext : IUserContext
    {
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext.User.Claims;

            UserId = Guid.NewGuid();
            AccountId = Guid.NewGuid();
            Name = Guid.NewGuid().ToString();
            AvatarUrl = Guid.NewGuid().ToString();
            Permissions = new List<Permission>();
        }

        public Guid UserId { get; }

        public Guid AccountId { get; }

        public string Name { get; }

        public string AvatarUrl { get; }

        public ICollection<Permission> Permissions { get; }

        public bool HasAny(params Permission[] permissions)
        {
            return permissions.Intersect(Permissions).Any();
        }

        public bool HasAll(params Permission[] permissions)
        {
            return !permissions.Except(Permissions).Any();
        }

        public bool Belongs(params Guid[] accountIds)
        {
            return accountIds.All(x => x == AccountId);
        }
    }
}