﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Companies.Models;
using Crm.Apps.Clients.Companies.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Companies.Clients
{
    public class CompanyAttributeChangesClient : ICompanyAttributeChangesClient
    {
        private readonly CompaniesClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public CompanyAttributeChangesClient(IOptions<CompaniesClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<CompanyAttributeChange>> GetPagedListAsync(Guid? changerUserId = default,
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

            return _httpClientFactory.PostAsync<List<CompanyAttributeChange>>(
                $"{_settings.Host}/Api/Companies/Attributes/Changes/GetPagedList", parameter, ct);
        }
    }
}