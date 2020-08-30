using Etain.WeatherForecast;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Eatin.WeatherForecast.Test
{
    public class IntegrationTests: IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public IntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        [Theory]
        [InlineData("/weatherforecast?startdate=2020-8-29&enddate=2020-9-2")]
        public async Task GetWeatherForecastWithoutCredentials_NotValid(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:52780");

            // Act
            // string url = "";
            var response = await client.GetAsync(url);

            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }
        [Theory]
        [InlineData("/weatherforecast?startdate=2020-8-29&enddate=2020-9-2")]
        [InlineData("/weatherforecast?startdate=2020-7-29&enddate=2020-8-2")]
        [InlineData("/weatherforecast?startdate=2020-9-1&enddate=2020-9-6")]
        public async Task GetWeatherForecastNextFiveDays_ValidAsync(string url)
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

            client.BaseAddress = new Uri("http://localhost:52780");

            // Act
            // string url = "";
            var response = await client.GetAsync(url);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
        [Theory]
        [InlineData("/weatherforecast?startdate=2020-8-29&enddate=2020-7-2")]
        [InlineData("/weatherforecast?startdate=2020-8-29")]
        public async Task GetWeatherForecastNextFiveDaysWrongParameters_NotValid(string url)
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

            client.BaseAddress = new Uri("http://localhost:52780");

            // Act
            // string url = "";
            var response = await client.GetAsync(url);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

    }


}

