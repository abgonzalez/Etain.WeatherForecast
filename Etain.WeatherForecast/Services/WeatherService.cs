using Etain.WeatherForecast.Infraestructure;
using Etain.WeatherForecast.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Etain.WeatherForecast.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly IConfiguration _configuration;
        public WeatherService(ILogger<WeatherService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<IEnumerable<WeatherResponse>> GetWeatherForecastsForLocationAsync(string location, DateTime startdate, DateTime enddate)
        {
            List<WeatherResponse> forecasts = new List<WeatherResponse>();
                var settings = _configuration.GetSection("Settings").Get<WebSettings>();

                using (var client = new HttpClient())
                {
                    var Url = settings.WeatherApi + location;

                    client.BaseAddress = new Uri(Url);

                    for (var date = startdate; date < enddate; date = date.AddDays(1))
                    {
                        var res = await client.GetAsync(date.ToString("yyyy/MM/dd"));
                        if (res.IsSuccessStatusCode)
                        {
                            var forecast = GetDataByDay(await res.Content.ReadAsStringAsync());
                            if (forecast != null)
                                forecasts.Add(forecast);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            return forecasts.ToArray();
        }

        private WeatherResponse GetDataByDay(string jsonlist)
        {
            var weatherResponseDef = new[]
            {
                new {
                    applicable_date = new DateTime(),
                    the_temp="",
                    weather_state_name="",
                    weather_state_abbr = ""
                }
            };

            var result = JsonConvert.DeserializeAnonymousType(jsonlist, weatherResponseDef);
            if (result.Count() ==0 )
            {
                return null;
                //return new WeatherResponse();
            }
                
            var average = result.GroupBy(x => x.weather_state_name)
                       .OrderByDescending(g => g.Count())
                       .FirstOrDefault().FirstOrDefault();

            var ImagesUrl = _configuration["Settings:Weather.Api.Images"];
            return new WeatherResponse()
            {
                Date = average.applicable_date,
                Temperature = double.Parse(average.the_temp, CultureInfo.InvariantCulture),
                Summary = average.weather_state_name,
                ImageUrl = $"{ImagesUrl}{average.weather_state_abbr}.png"
            };
        }
    }
}
