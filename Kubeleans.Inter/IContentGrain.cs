using System.Threading.Tasks;
using Orleans;

namespace Kubeleans.Inter
{
    public interface IContentGrain : IGrainWithStringKey
    {
        Task<string> GetFoodBio();
    }
}