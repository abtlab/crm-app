﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Leads.Models;
using Crm.Clients.Leads.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Leads.Clients
{
    public class LeadAttributeChangesClient : ILeadAttributeChangesClient
    {
        private readonly LeadsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadAttributeChangesClient(IOptions<LeadsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<LeadAttributeChange>> GetPagedListAsync(Guid? changerUserId = default,
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

            return _httpClientFactory.PostAsync<List<LeadAttributeChange>>(
                $"{_settings.Host}/Api/Leads/Attributes/Changes/GetPagedList", parameter, ct);
        }
    }
}