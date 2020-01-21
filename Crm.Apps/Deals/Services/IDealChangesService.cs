﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.v1.Models;
using Crm.Apps.Deals.v1.RequestParameters;

namespace Crm.Apps.Deals.Services
{
    public interface IDealChangesService
    {
        Task<List<DealChange>> GetPagedListAsync(DealChangeGetPagedListRequestParameter request, CancellationToken ct);
    }
}