import React, { Component } from 'react';
import PropTypes from 'prop-types';

import {
    Wrapper,
    Status,
    FieldWrapper,
    Label,
    ReadOnly,
    Input,
    ButtonWrapper,
    Button
  } from '@styles/controls';

class DashboardComponent extends Component {
  state = {};

  static propTypes = {
    onAction: PropTypes.func,
    email: PropTypes.string
  }

  static defaultProps = {
    onAction: (data) => {}
  }

  constructor(props) {
    super(props);

    this.state = {
      busy: false,
      error: false,
      status: '',
      search: ''
    };
  }

  onSearch = () => {
    this.props.onAction(this.state.search, 'search');
  }

  onLogout = () => {
    this.props.onAction(null, 'logout');
  }

  onSearchChange = (event) => {
    this.setState({search: event.target.value});
  }

  showBusy() {
    this.setState({
      busy: true,
      status: 'Please wait...',
      error: false
    });
  }

  showError(message) {
    this.setState({
      error: true,
      busy: false,
      status: message,
    });
  }

  showStatus() {
    return this.state.busy || this.state.error;
  }

  render() {
    return (
      <Wrapper>
        <FieldWrapper>
          <Label>Email:</Label>
          <ReadOnly>{this.props.email}</ReadOnly>
        </FieldWrapper>
        <FieldWrapper>
          <Label>Search:</Label>
          <Input value={this.state.search} onChange={this.onSearchChange} />
        </FieldWrapper>

        <Status show={this.showStatus()} error={this.state.error}>{this.state.status}</Status>

        <ButtonWrapper>
          <Button onClick={this.onSearch} disabled={this.state.busy}>Search</Button>
          <Button onClick={this.onLogout} disabled={this.state.busy}>Log Out</Button>
        </ButtonWrapper>
      </Wrapper>
    );
  }
}

export default DashboardComponent;