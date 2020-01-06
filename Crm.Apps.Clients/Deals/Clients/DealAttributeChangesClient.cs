﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.RequestParameters;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.Deals.Clients
{
    public class DealAttributeChangesClient : IDealAttributeChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public DealAttributeChangesClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Deals/Attributes/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<DealAttributeChange>> GetPagedListAsync(
            DealAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<DealAttributeChange>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }
    }
}