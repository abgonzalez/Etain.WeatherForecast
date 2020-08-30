using System.Collections.Generic;

namespace Etain.WeatherForecast.Infraestructure
{
    public class WebSettings
    {
        public string WeatherApi { get; set; }
        public string WeatherApiImages { get; set; }
        public int ForecastNumOfDays { get; set; }

        public Dictionary<string, string> Location { get; set; }
    }
}


