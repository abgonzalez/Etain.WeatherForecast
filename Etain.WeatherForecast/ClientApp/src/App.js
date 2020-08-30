import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';

import { WeatherForecast } from './components/WeatherForecast';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
        <AuthorizeRoute exact path='/' component={WeatherForecast} />
      </Layout>
    );
  }
}
