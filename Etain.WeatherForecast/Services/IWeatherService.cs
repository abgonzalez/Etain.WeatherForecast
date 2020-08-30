using Etain.WeatherForecast.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etain.WeatherForecast.Service
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherResponse>> GetWeatherForecastsForLocationAsync(string locationId, DateTime startdate, DateTime enddate);
    }
}