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
    Button,
    TextWrapper,
    GoogleMarker,
    Caret
  } from '@styles/controls';

class DashboardComponent extends Component {
  state = {};

  static propTypes = {
    onAction: PropTypes.func,
    email: PropTypes.string,
    status: PropTypes.string,
    error: PropTypes.bool
  }

  static defaultProps = {
    onAction: (data) => {}
  }

  constructor(props) {
    super(props);

    this.state = {
      busy: false,
      error: props.error || false,
      status: props.status || '',
      search: '',
      expand: true
    };

    this.onSearch = this.onSearch.bind(this);
    this.onLogout = this.onLogout.bind(this);
    this.onSearchChange = this.onSearchChange.bind(this);
    this.onCaretClick = this.onCaretClick.bind(this);
  }

  componentDidUpdate(prevProps) {
    if (this.props.error !== prevProps.error) {
      this.setState({
        error: this.props.error,

        // operation is done and parent component is trying to display a status
        busy: false,
        force: true
      });
    }

    if (this.props.status !== prevProps.status) {
      this.setState({
        status: this.props.status,

        // operation is done and parent component is trying to display a status
        busy: false,
        force: true
      });
    }
  }

  onSearch = () => {
    this.showBusy();
    this.props.onAction(this.state.search, 'search');
  }

  onLogout = () => {
    this.showBusy();
    this.props.onAction(null, 'logout');
  }

  onSearchChange = (event) => {
    this.setState({search: event.target.value});
  }

  onCaretClick = () => {
    this.setState({expand: !this.state.expand});
  }

  showBusy() {
    this.setState({
      busy: true,
      status: 'Please wait...',
      error: false,
      force: false
    });
  }

  showError(message) {
    this.setState({
      error: true,
      busy: false,
      status: message,
      force: false
    });
  }

  showStatus() {
    return this.state.busy || this.state.error || this.state.force;
  }

  render() {
    return (
      <Wrapper>
        <FieldWrapper>
          <Label>Email:</Label>
          <ReadOnly>{this.props.email}</ReadOnly>
        </FieldWrapper>
        <FieldWrapper>
          <Label>Filter:</Label>
          <Input value={this.state.search} onChange={this.onSearchChange} />
          <Button onClick={this.onSearch} disabled={this.state.busy}>Apply</Button>
        </FieldWrapper>

        <ButtonWrapper>
          <Button onClick={this.onLogout} disabled={this.state.busy}>Log Out</Button>
          <Status show={this.showStatus()} error={this.state.error}>{this.state.status}</Status>
        </ButtonWrapper>

        <div>
          <TextWrapper>
            <Caret expand={this.state.expand} onClick={this.onCaretClick} />
            <div onClick={this.onCaretClick}>{!this.state.expand ? 'Instructions:' : 'Click to view instructions'}</div>
          </TextWrapper>

          {!this.state.expand && (
            <ul>
              <li>Apply filter to search for specific user or remark</li>
              <li>Click on <GoogleMarker color={'red'} size={'20px'} /> to update or delete your remark</li>
              <li>Click on <GoogleMarker color={'blue'} size={'20px'} /> to view other user's remark</li>
              <li>Double click on map to add a new remark <GoogleMarker color={'yellow'} size={'20px'} /> on that location</li>
              <li>Right click on map to center on selected remark's <GoogleMarker color={'green'} size={'20px'} /> location. If there is none selected, center on current location.</li>
            </ul>
          )}
        </div>
      </Wrapper>
    );
  }
}

export default DashboardComponent;