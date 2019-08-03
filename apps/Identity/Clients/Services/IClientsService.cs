using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Identity.Clients.Models;
using Identity.Clients.Parameters;

namespace Identity.Clients.Services
{
    public interface IClientsService
    {
        Task<Client> GetAsync(Guid id, CancellationToken ct);

        Task<Client> GetByClientIdAsync(string clientId, CancellationToken ct);

        Task<Client[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<Client[]> GetPagedListAsync(ClientGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Client client, CancellationToken ct);

        Task UpdateAsync(Client oldClient, Client client, CancellationToken ct);

        Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct);
    }
}