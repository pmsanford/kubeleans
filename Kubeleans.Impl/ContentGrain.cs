using System.Threading.Tasks;
using Kubeleans.Inter;
using Orleans;

namespace Kubeleans.Impl
{
    public class ContentGrain : Grain, IContentGrain
    {
        private readonly IConfig config;

        public ContentGrain(IConfig config)
        {
            this.config = config;
        }

        public async Task<string> GetFoodPane()
        {
            var db = GrainFactory.GetGrain<IDatabaseGrain>(config.DatabaseName);
            var food = await db.GetFavoriteFood(this.GetPrimaryKeyString());
            return $"I am <strong>{this.GetPrimaryKeyString()}</strong> and I love {food}";
        }
    }
}