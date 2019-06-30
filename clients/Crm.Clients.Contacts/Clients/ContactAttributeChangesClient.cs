﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Contacts.Models;
using Crm.Clients.Contacts.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Contacts.Clients
{
    public class ContactAttributeChangesClient : IContactAttributeChangesClient
    {
        private readonly ContactsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ContactAttributeChangesClient(IOptions<ContactsClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<ContactAttributeChange>> GetPagedListAsync(Guid? changerUserId = default,
            Guid? attributeId = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                ChangerUserId = changerUserId,
                AttributeId = attributeId,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<ContactAttributeChange>>(
                $"{_settings.Host}/Api/Contacts/Attributes/Changes/GetPagedList", parameter, ct);
        }
    }
}