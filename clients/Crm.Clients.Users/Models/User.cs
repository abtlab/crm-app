﻿using System;
using System.Collections.Generic;
using Crm.Common.UserContext;

namespace Crm.Clients.Users.Models
{
    public class User
    {
        public Guid Id { get; set; }
        
        public Guid AccountId { get; set; }
        
        public string Surname { get; set; }
        
        public string Name { get; set; }
        
        public string Patronymic { get; set; }
        
        public DateTime? BirthDate { get; set; }
        
        public UserGender Gender { get; set; }
        
        public string AvatarUrl { get; set; }
        
        public bool IsLocked { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public DateTime CreateDateTime { get; set; }
        
        public List<UserAttributeLink> AttributeLinks { get; set; }
        
        public List<UserPermission> Permissions { get; set; }
        
        public List<UserGroupLink> GroupLinks { get; set; }

        public List<UserSetting> Settings { get; set; }
    }
}