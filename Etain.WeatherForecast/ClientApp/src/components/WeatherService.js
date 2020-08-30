import authService from './api-authorization/AuthorizeService'
const weatherApiUrl = "weatherforecast?";

export class WeatherService {
 
    async getForecastRange(startDate, endDate) {
        debugger;
        let query = `${weatherApiUrl}startdate=${getFormatedDate(startDate)}&enddate=${getFormatedDate(endDate)}`;
        const token = await authService.getAccessToken();
        let response = await fetch(query, {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
          });
        return await response.json();
    }

    static get instance() { return weatherService }
}

const getFormatedDate = (date) => {
    return `${date.getFullYear()}-${date.getMonth()+1}-${date.getDate()}`;
}

const weatherService = new WeatherService();

export default weatherService;

