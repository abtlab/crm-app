using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Models;
using Crm.Apps.Clients.Users.RequestParameters;

namespace Crm.Apps.Clients.Users.Clients
{
    public interface IUserAttributeChangesClient
    {
        Task<List<UserAttributeChange>> GetPagedListAsync(
            UserAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}