using Etain.WeatherForecast.Infraestructure;
using Etain.WeatherForecast.Models;
using Etain.WeatherForecast.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Eatin.WeatherForecast.Test
{
    public class WeatherServiceTest
    {
        private Mock<ILogger<WeatherService>>  _mockLogger ;
        private IConfiguration _configuration;
        public WeatherServiceTest()
        {
            _mockLogger = new Mock<ILogger<WeatherService>>();
            _configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", false, true)
                                .Build();
        }

        [Fact]
        public async Task GetWeatherForecastNext5DaysBelfast_ShouldGetOK()
        {
            //Arrange
            var service = new WeatherService(_mockLogger.Object, _configuration);
            var settings = _configuration.GetSection("Settings").Get<WebSettings>();
            settings.Location.TryGetValue("Default", out string location);

            // Act
            var result = await service.GetWeatherForecastsForLocationAsync(location, DateTime.Now, DateTime.Now.AddDays(4));

            // Assert
            var checkType = Assert.IsType<WeatherResponse[]>(result);
            Assert.Equal(5, result.Count());
        }
        [Fact]
        public async Task GetWeatherForecastNext5DaysNonExistentLocation_ShouldGetNotData()
        {
            //Arrange
            var service = new WeatherService(_mockLogger.Object, _configuration);

            string location = "Xfdsfsdfs";
            // Act
            var result = await service.GetWeatherForecastsForLocationAsync(location, DateTime.Now, DateTime.Now.AddDays(5));

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetWeatherForecastWithWrongDatesInBelfast_ShouldGetNotData()
        {
            //Arrange
            var service = new WeatherService(_mockLogger.Object, _configuration);
            var settings = _configuration.GetSection("Settings").Get<WebSettings>();
            settings.Location.TryGetValue("Default", out string location);
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now.AddDays(-5);

            // Act
            var result = await service.GetWeatherForecastsForLocationAsync(location, startdate, enddate);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetWeatherForecastWithPastDatesInBelfast_ShouldGetData()
        {
            //Arrange
            var service = new WeatherService(_mockLogger.Object, _configuration);
            var settings = _configuration.GetSection("Settings").Get<WebSettings>();
            settings.Location.TryGetValue("Default", out string location);
            DateTime startdate = DateTime.Now.AddDays(-10);
            DateTime enddate = DateTime.Now.AddDays(-6);

            // Act
            var result = await service.GetWeatherForecastsForLocationAsync(location, startdate, enddate);

            // Assert
            var checkType = Assert.IsType<WeatherResponse[]>(result);
            Assert.Equal(5, result.Count());

        }
    }
}
