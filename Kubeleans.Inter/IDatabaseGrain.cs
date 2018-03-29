using System.Threading.Tasks;
using Orleans;

namespace Kubeleans.Inter
{
    public interface IDatabaseGrain : IGrainWithStringKey
    {
        Task<string> GetFavoriteFood(string name);
    }
}