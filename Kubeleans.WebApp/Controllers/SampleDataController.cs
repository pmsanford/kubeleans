using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kubeleans.Inter;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace Kubeleans.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static string[] Names = new string[] {
            "Paul", "Dan", "Jon", "Greg", "Mark", "Ann"
        };
        private readonly IClusterClient client;

        public SampleDataController(IClusterClient client)
        {
            this.client = client;
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<FoodDatum>> FoodData()
        {
            var ret = new List<FoodDatum>();
            foreach (var name in Names)
            {
                var grain = client.GetGrain<IContentGrain>(name);
                var content = await grain.GetFoodPane();
                ret.Add(new FoodDatum
                {
                    Name = name,
                    Content = content
                });
            }
            return ret;
        }

        public class FoodDatum
        {
            public string Name { get; set; }
            public string Content { get; set; }
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
