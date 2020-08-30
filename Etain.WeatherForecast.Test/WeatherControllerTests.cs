using Etain.WeatherForecast.Controllers;
using Etain.WeatherForecast.Infraestructure;
using Etain.WeatherForecast.Models;
using Etain.WeatherForecast.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Eatin.WeatherForecast.Test
{
    public class WeatherControllerTests
    {
        private Mock<ILogger<WeatherService>> _mockServiceLogger;
        private IConfiguration _configuration;
        private WeatherService _weatherService;
        private Mock<ILogger<WeatherForecastController>> _mockControllerLogger;
        public WeatherControllerTests()
        {
            _mockServiceLogger = new Mock<ILogger<WeatherService>>();
            _configuration = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", false, true)
                                 .Build();
            _weatherService = new WeatherService(_mockServiceLogger.Object, _configuration);
            _mockControllerLogger = new Mock<ILogger<WeatherForecastController>>();
        }
        [Fact]
        public async Task GetWeatherForecastNext5DaysBelfast_ShouldGetOK()
        {
            //Arrange
            var controller = new WeatherForecastController(_mockControllerLogger.Object,
                                                            _configuration, 
                                                            _weatherService);

            // Act
            var result = await controller.Get(DateTime.Now, DateTime.Now.AddDays(5));

            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<WeatherResponse>>>(result);
            var modelObject = Assert.IsAssignableFrom<Microsoft.AspNetCore.Mvc.OkObjectResult>(viewResult.Result);
            var modelValue = Assert.IsAssignableFrom<IEnumerable<WeatherResponse>>(modelObject.Value);
            Assert.Equal(StatusCodes.Status200OK, modelObject.StatusCode);
            Assert.Equal(6, modelValue.Count());
        }
        [Fact]
        public async Task GetWeatherForecastWithPastDatesInBelfast_ShouldGetBadRequest()
        {
            //Arrange
            var controller = new WeatherForecastController(_mockControllerLogger.Object,
                                                _configuration,
                                                _weatherService);


            // Act
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now.AddDays(-5);
            var result = await controller.Get(startdate, enddate);


            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<WeatherResponse>>>(result);
            var modelObject = Assert.IsAssignableFrom<Microsoft.AspNetCore.Mvc.BadRequestResult>(viewResult.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, modelObject.StatusCode);
        }
        [Fact]
        public async Task GetWeatherForecastIn5YearsTimeInBelfast_ShouldGetNotDataFound()
        {
            //Arrange
            var controller = new WeatherForecastController(_mockControllerLogger.Object,
                                                _configuration,
                                                _weatherService);


            // Act
            DateTime startdate = DateTime.Now.AddYears(3);
            DateTime enddate = startdate.AddDays(5);
            var result = await controller.Get(startdate, enddate);


            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<WeatherResponse>>>(result);
            var modelObject = Assert.IsAssignableFrom<Microsoft.AspNetCore.Mvc.StatusCodeResult>(viewResult.Result);
            Assert.Equal(StatusCodes.Status404NotFound, modelObject.StatusCode);
        }
       
    }
}
