using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;

namespace Crm.Apps.Tests.Builders.Deals
{
    public interface IDealStatusBuilder
    {
        DealStatusBuilder WithName(string name);

        DealStatusBuilder AsFinish();

        DealStatusBuilder AsDeleted();

        Task<DealStatus> BuildAsync();
    }
}