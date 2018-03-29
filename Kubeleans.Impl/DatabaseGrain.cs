using System.Linq;
using System.Threading.Tasks;
using Kubeleans.Inter;
using Orleans;

namespace Kubeleans.Impl
{
    public class DatabaseGrain : Grain, IDatabaseGrain
    {
        public Task<string> GetFavoriteFood(string name)
        {
            var sum = name.Select(chr => (int)chr).Sum();
            var food = "bananas";
            if (sum % 5 == 0)
            {
                food = "apples";
            }
            else if (sum % 3 == 0)
            {
                food = "oates";
            }
            return Task.FromResult(food);
        }
    }
}