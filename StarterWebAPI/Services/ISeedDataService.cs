using System.Threading.Tasks;

namespace StarterAPI.Services
{
    public interface ISeedDataService
    {
        Task EnsureSeedData();
    }
}
