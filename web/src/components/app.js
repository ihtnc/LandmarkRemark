import React, { Component } from 'react';

import session from '@src/session';
import AppWrapper from '@styles/appwrapper';
import ApiClient from '@api/apiClient';

import LandmarkMap from './landmarkMapComponent';
import Login from './loginComponent';
import Dashboard from './dashboardComponent';

class AppComponent extends Component {
  state = {};

  constructor(props) {
    super(props);

    this.state.remarks = null;
  }

  componentDidMount() {
    var { isLoggedIn } = session.getUser();

    // if user is logged in but there are no remarks yet, fetch remarks
    if(isLoggedIn && !this.state.remarks) {
      this.searchRemarks();
    }
  }

  onLogin = (data) => {
    session.start(data);

    this.searchRemarks();
  }

  onSearch = (data, action) => {
    if(action == 'logout') {
      session.stop();
      this.setState({ remarks: null });
      return;
    }

    this.searchRemarks(data);
  }

  searchRemarks(text) {
    ApiClient
      .getRemarks(text)
      .then(res => {
        this.setState({ remarks: res.data.data });
      })
      .catch(error => {
        this.setState({ remarks: null });
      });
  }

  render() {
    var { email, isLoggedIn } = session.getUser();

    return (
      <AppWrapper>
        { !isLoggedIn && (<Login onAction={this.onLogin}></Login>)}
        { isLoggedIn && (<Dashboard email={email} onAction={this.onSearch}></Dashboard>)}

        <LandmarkMap remarks={this.state.remarks}></LandmarkMap>
      </AppWrapper>
    );
  }
}

export default AppComponent;