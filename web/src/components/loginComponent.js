import React, { Component } from 'react';
import PropTypes from 'prop-types';

import {
    Wrapper,
    Status,
    FieldWrapper,
    Label,
    Input,
    EmailValidationIcon,
    PasswordStrengthIcon,
    ButtonWrapper,
    Button
  } from '@styles/controls';

import ApiClient from '@api/apiClient';

class LoginComponent extends Component {
  state = {};

  static propTypes = {
    onAction: PropTypes.func
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
      email: '',
      password: '',
      confirm: '',
      register: false
    };
  }

  onRegister = () => {
    this.showBusy();

    if(this.state.password != this.state.confirm) {
      this.showError('Passwords must match.');
      return;
    }

    ApiClient.register({
        email: this.state.email,
        password: this.state.password
      }).then(res => {
        const data = res.data.data;
        this.props.onAction(data);
      })
      .catch(error => {
        this.showError(error.message);
      });
  }

  onLogin = () => {
    this.showBusy();

    ApiClient.login({
        email: this.state.email,
        password: this.state.password
      }).then(res => {
        const data = res.data.data;
        this.props.onAction(data);
      })
      .catch(error => {
        this.showError(error.message);
      });
  }

  onNewUser = () => {
    this.setState({register: true });
    this.clearForm();
  }

  onCancel = () => {
    this.setState({ register: false });
    this.clearForm();
  }

  onEmailChange = (event) => {
    this.setState({email: event.target.value});
  }

  onPasswordChange = (event) => {
    this.setState({password: event.target.value});
  }

  onConfirmChange = (event) => {
    this.setState({confirm: event.target.value});
  }

  clearForm() {
    this.setState({
      email: '',
      password: '',
      confirm: ''
    });
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
          <Input value={this.state.email} onChange={this.onEmailChange} />
          {this.state.register && (
            <EmailValidationIcon value={this.state.email} />
          )}
        </FieldWrapper>

        <FieldWrapper>
          <Label>Password:</Label>
          <Input type={'password'} value={this.state.password} onChange={this.onPasswordChange} />
          {this.state.register && (
            <PasswordStrengthIcon value={this.state.password} />
          )}
        </FieldWrapper>

        {this.state.register && (
          <FieldWrapper>
            <Label>Confirm:</Label>
            <Input type={'password'} value={this.state.confirm} onChange={this.onConfirmChange} />
          </FieldWrapper>
        )}

        <Status show={this.showStatus()} error={this.state.error}>{this.state.status}</Status>

        <ButtonWrapper>
          {this.state.register && (<Button onClick={this.onCancel} disabled={this.state.busy}>Cancel</Button>)}
          {this.state.register && (<Button onClick={this.onRegister} disabled={this.state.busy}>Register</Button>)}
          {!this.state.register && (<Button onClick={this.onNewUser} disabled={this.state.busy}>Register</Button>)}
          {!this.state.register && (<Button onClick={this.onLogin} disabled={this.state.busy}>Login</Button>)}
        </ButtonWrapper>
      </Wrapper>
    );
  }
}

export default LoginComponent;