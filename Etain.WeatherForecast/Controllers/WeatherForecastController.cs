using Etain.WeatherForecast.Service;
using Etain.WeatherForecast.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Etain.WeatherForecast.Infraestructure;
using System.Linq;

namespace Etain.WeatherForecast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWeatherService _weatherService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                         IConfiguration configuration,
                                         IWeatherService weatherService)
        {
            _logger = logger;
            _configuration = configuration;
            _weatherService = weatherService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad input data
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Error Server
        [ProducesResponseType(StatusCodes.Status200OK)] //OK
        public async Task<ActionResult<IEnumerable<WeatherResponse>>> Get(DateTime startdate, DateTime enddate)
        {
            var settings = _configuration.GetSection("Settings").Get<WebSettings>();

            if (enddate == null)
                enddate = startdate.AddDays(settings.ForecastNumOfDays);

            if (enddate < startdate)
                return BadRequest();
            settings.Location.TryGetValue("Default", out string location);
            if (String.IsNullOrEmpty(location))
                return BadRequest();

            if (String.IsNullOrEmpty(settings.WeatherApi))
                return BadRequest();

            var result = await _weatherService.GetWeatherForecastsForLocationAsync(location, startdate, enddate);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            if (result.Count() == 0)
                return StatusCode(StatusCodes.Status404NotFound);
            return Ok(result);
        }
    }
}
