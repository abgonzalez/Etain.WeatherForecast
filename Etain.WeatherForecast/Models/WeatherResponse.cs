using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Etain.WeatherForecast.Models
{ 
    public class WeatherResponse
    {

        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public string ImageUrl { get; set; }

        public string Summary { get; set; }
    }
}
