using System.Threading.Tasks;
using Kubeleans.Inter;
using Orleans;
using Orleans.Runtime;

namespace Kubeleans.Impl
{
    public class ContentGrain : Grain, IContentGrain
    {
        private readonly IConfig config;
        private readonly ISiloInfo info;

        public ContentGrain(IConfig config, ISiloInfo info)
        {
            this.config = config;
            this.info = info;
        }

        public async Task<string> GetFoodBio()
        {
            var db = GrainFactory.GetGrain<IDatabaseGrain>(config.DatabaseName);
            var food = await db.GetFavoriteFood(this.GetPrimaryKeyString());
            return $"I am {this.GetPrimaryKeyString()} and I love {food} when I'm on silo {info.SiloID}";
        }
    }
}