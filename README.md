# Etain WeatherForecast Test

# Problem
Create an ASP.NET MVC application that will display the weather forecast for the next 5 days in Belfast.

# Proposed Solution
Create an authenticated web application using React where only authorised users can access the weather forecast. Use out of the box authentication mechanisms.
The application will communicate to the following REST API: https://www.metaweather.com/api/
and retrieve the next 5 days of weather for Belfast.
Create a list containing the weather forecast for each of the next 5 days, this will include the date,
the weather state and an image to represent the weather state.
The application will include the ability to pull to refresh, which will update with the latest forecast,
which in turn updates the list.
# Hints
URL for snow image can be found here:
https://www.metaweather.com/static/img/weather/png/sn.png
Replace the weather state abbreviation (sn) in the image filename with the desired state.

## Setup Notes

In order to get the app up and running, just follow the steps below:

1. Clone the repo to your local development machine.
2. Open a command prompt and go to the directory `WeatherForecast-master\Etain.WeatherForecast`.
3. Run `dotnet ef database update` to create the local database in your pc.
4. Run `ddotnet run` to start the web.
5. User can access to the web in http://localhost:5005 or https://localhost:500665. The first time, it's necessary to register the user and confirm the email. 
6. Now, you can see the weather forecast in Belfast for the next 5 days :-) 

## Tech Stack
- Asp.Net Core 3.1
- AspNetCore.Identity with Local database
- React


