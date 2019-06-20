﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Products.Settings;
using Crm.Clients.Products.Models;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Products.Clients
{
    public class ProductStatusesClient : IProductStatusesClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ProductsClientSettings _settings;

        public ProductStatusesClient(IOptions<ProductsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<ProductStatus> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ProductStatus>($"{_settings.Host}/Api/Products/Statuses/Get",
                new {id}, ct);
        }

        public Task<List<ProductStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<ProductStatus>>(
                $"{_settings.Host}/Api/Products/Statuses/GetList", ids, ct);
        }

        public Task<List<ProductStatus>> GetPagedListAsync(Guid? accountId = default, string name = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                AccountId = accountId,
                Name = name,
                IsDeleted = isDeleted,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<ProductStatus>>(
                $"{_settings.Host}/Api/Products/Statuses/GetPagedList",
                parameter, ct);
        }

        public Task<Guid> CreateAsync(ProductStatus group, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Products/Statuses/Create", group, ct);
        }

        public Task UpdateAsync(ProductStatus group, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Statuses/Update", group, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Statuses/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Statuses/Restore", ids, ct);
        }
    }
}