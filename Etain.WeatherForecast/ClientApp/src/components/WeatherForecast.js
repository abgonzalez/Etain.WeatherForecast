import React from 'react';
import ReactPullToRefresh from 'react-pull-to-refresh';
import weatherService from './WeatherService';

import './WeatherForecast.css';

export class WeatherForecast extends React.Component{

    constructor(props) {
        super(props);
        this.state = {
            forecasts:[], loading:true
        };
      }

    async getForecasts() {
        this.setState({loading:true});
        let startdate = new Date();
        let enddate = new Date();
        enddate.setDate(startdate.getDate() + 5);
        let data = await weatherService.getForecastRange(startdate, enddate);
        this.setState({forecasts:data, loading:false});
    }

    componentDidMount(){
        this.getForecasts();
    }

    render(){
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : <table class="table table-striped">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temperature</th>
                        <th>Summary</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                {this.state.forecasts.map((x,index)=><WeatherCard {...x}/>)}
                </tbody>
            </table>

        return(
            <div>
                <ReactPullToRefresh onRefresh={()=>this.getForecasts()}>                  
                    <h2>Weather Forecast in Belfast</h2>
                    <div>
                        {contents}
                    </div>     
                </ReactPullToRefresh>
            </div>
        )
    }
}

const WeatherCard = (props) => {
    var dateObj = new Date(props.date);
    return (
        <tr>
            <td>{dateObj.toDateString()}</td>
            <td>{props.temperature.toFixed(2)}</td>
            <td>
                {props.summary}
            </td>
            <td>
              <img className="img" alt="" src={props.imageUrl} />
            </td>
        </tr>
    )  
}