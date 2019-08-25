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

    this.state = {
      remarks: null,
      error: false,
      message: ''
    };

    this.onLogin = this.onLogin.bind(this);
    this.onDashboardAction = this.onDashboardAction.bind(this);
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

  onDashboardAction = (data, action) => {
    if(action == 'logout') {
      session.stop();
      this.setState({ remarks: null });
      return;
    }

    if(action == 'search') {
      this.searchRemarks(data);
      return;
    }

    if(action == 'next') {
      const nextIndex = (this.state.remarkIndex + 1) % this.state.remarks.length;
      const remark = this.state.remarks[nextIndex];

      this.setState({
        remarkIndex: nextIndex,
        remarkPosition: { lat: remark.latitude, lng: remark.longitude }
      });

      return;
    }

    if(action == 'prev') {
      const prevIndex = (this.state.remarks.length + this.state.remarkIndex - 1) % this.state.remarks.length;
      const remark = this.state.remarks[prevIndex];

      this.setState({
        remarkIndex: prevIndex,
        remarkPosition: { lat: remark.latitude, lng: remark.longitude }
      });

      return;
    }
  }

  searchRemarks(text) {
    this.showStatus('', false);

    ApiClient
      .getRemarks(text)
      .then(res => {
        this.setState({
          remarks: res.data.data,
          remarkIndex: 0,
          remarkPosition: null
        });
        this.showStatus(`${res.data.data.length} remark(s) found.`, false);
      })
      .catch(error => {
        this.setState({ remarks: null });
        this.showStatus(error.message, true);
      });
  }

  showStatus(message, error) {
    this.setState({
      error: error,
      message: message
    });
  }

  render() {
    var { email, isLoggedIn } = session.getUser();

    return (
      <AppWrapper>
        { !isLoggedIn && (<Login onAction={this.onLogin}></Login>)}
        { isLoggedIn && (<Dashboard status={this.state.message} error={this.state.error} email={email} onAction={this.onDashboardAction}></Dashboard>)}

        <LandmarkMap center={this.state.remarkPosition} remarks={this.state.remarks}></LandmarkMap>
      </AppWrapper>
    );
  }
}

export default AppComponent;