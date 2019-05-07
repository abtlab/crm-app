﻿using Crm.Apps.Areas.Users.Models;
using Crm.Infrastructure.Orm;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Areas.Users.Storages
{
    public class UsersStorage : DbContext
    {
        private readonly OrmSettings _config;

        public DbSet<User> Users { get; set; }

        public DbSet<UserAttribute> UserAttributes { get; set; }

        public DbSet<UserAttributeChange> UserAttributeChanges { get; set; }

        public DbSet<UserAttributeLink> UserAttributeLinks { get; set; }

        public DbSet<UserChange> UserChanges { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<UserGroupChange> UserGroupChanges { get; set; }

        public DbSet<UserGroupLink> UserGroupLinks { get; set; }

        public DbSet<UserGroupPermission> UserGroupPermissions { get; set; }

        public DbSet<UserSetting> UserSettings { get; set; }

        public UsersStorage(IOptions<OrmSettings> options)
        {
            _config = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(_config.MainConnectionString);
        }
    }
}