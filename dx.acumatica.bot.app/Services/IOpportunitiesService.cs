using System.Threading.Tasks;
using dx.acumatica.bot.app.Models;

namespace dx.acumatica.bot.app.Services
{
    public interface IOpportunitiesService
    {
        Task<string> GetOpportunities(OpportunitiesRequestModel model);
    }
}