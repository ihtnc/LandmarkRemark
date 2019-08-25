import React, { Component } from 'react';
import PropTypes from 'prop-types';

import UserPreference from '@src/userPreference';

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
    InlineTextWrapper,
    GoogleMarker,
    Caret,
    Arrow
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
    onAction: (data, action) => {}
  }

  constructor(props) {
    super(props);

    this.state = {
      busy: false,
      error: props.error || false,
      status: props.status || '',
      search: '',
      collapseDashboard: UserPreference.get('collapseDashboard') == 'true',
      expandInfo: true
    };

    this.onSearch = this.onSearch.bind(this);
    this.onLogout = this.onLogout.bind(this);
    this.onSearchChange = this.onSearchChange.bind(this);
    this.onDashboardCaretClick = this.onDashboardCaretClick.bind(this);
    this.onInfoCaretClick = this.onInfoCaretClick.bind(this);
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

  onPreviousRemark = () => {
    this.props.onAction(null, 'prev');
  }

  onNextRemark = () => {
    this.props.onAction(null, 'next');
  }

  onLogout = () => {
    this.showBusy();
    this.props.onAction(null, 'logout');
  }

  onSearchChange = (event) => {
    this.setState({search: event.target.value});
  }

  onDashboardCaretClick = () => {
    UserPreference.save('collapseDashboard', !this.state.collapseDashboard);
    this.setState({collapseDashboard: !this.state.collapseDashboard});
  }

  onInfoCaretClick = () => {
    this.setState({expandInfo: !this.state.expandInfo});
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
        <TextWrapper>
          <Caret expand={this.state.collapseDashboard} onClick={this.onDashboardCaretClick} />
          <div onClick={this.onDashboardCaretClick}>Dashboard</div>
          {this.state.collapseDashboard && (
            <InlineTextWrapper>
              <div>:: {`(${this.props.email})`}</div>
              <Status show={this.showStatus()} error={this.state.error}>{this.state.status}</Status>
            </InlineTextWrapper>
          )}
        </TextWrapper>

        {!this.state.collapseDashboard && (
          <FieldWrapper>
            <Label>Email:</Label>
            <ReadOnly>{this.props.email}</ReadOnly>
          </FieldWrapper>
        )}

        <FieldWrapper>
          <Label>Filter:</Label>
          <Input value={this.state.search} onChange={this.onSearchChange} />
          <Button onClick={this.onSearch} disabled={this.state.busy}>Apply</Button>
          <Button onClick={this.onPreviousRemark} disabled={this.state.busy}>&lt;&lt;</Button>
          <Button onClick={this.onNextRemark} disabled={this.state.busy}>&gt;&gt;</Button>
        </FieldWrapper>

        {!this.state.collapseDashboard && (
          <ButtonWrapper>
            <Button onClick={this.onLogout} disabled={this.state.busy}>Log Out</Button>
            <Status show={this.showStatus()} error={this.state.error}>{this.state.status}</Status>
          </ButtonWrapper>
        )}

        {!this.state.collapseDashboard && (
          <div>
            <TextWrapper>
              <Caret expand={this.state.expandInfo} onClick={this.onInfoCaretClick} />
              <div onClick={this.onInfoCaretClick}>Instructions</div>
            </TextWrapper>

            {!this.state.expandInfo && (
              <ul>
                <li>Apply filter to search for specific user or remark.</li>
                <li>Click on <Arrow direction={'left'} size={'20px'} /> or <Arrow direction={'right'} size={'20px'} /> to navigate between remarks.</li>
                <li>Click on <GoogleMarker color={'red'} size={'20px'} /> to update or delete your remark.</li>
                <li>Click on <GoogleMarker color={'blue'} size={'20px'} /> to view other user's remark.</li>
                <li>Double click on map to add a new remark <GoogleMarker color={'yellow'} size={'20px'} /> on that location.</li>
                <li>Right click on map to center on selected remark's <GoogleMarker color={'green'} size={'20px'} /> location. If there is none selected, center on current location.</li>
              </ul>
            )}
          </div>
        )}
      </Wrapper>
    );
  }
}

export default DashboardComponent;