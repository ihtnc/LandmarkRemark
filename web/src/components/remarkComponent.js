import React, { Component } from 'react';
import PropTypes from 'prop-types';

import {
  Wrapper,
  Header,
  Footer,
  ReadOnly,
  Input,
  ButtonWrapper,
  Button
} from '@styles/controls';

import ApiClient from '@api/apiClient';

class RemarkComponent extends Component {
  state = {};

  static propTypes = {
    data: PropTypes.object,
    editable: PropTypes.bool,
    new: PropTypes.bool,
    onAction: PropTypes.func
  }

  static defaultProps = {
    data: { },
    editable: false,
    new: false,
    onAction: (data, action) => {}
  }

  constructor(props) {
    super(props);

    this.state = {
      remark: props.data.remark || '',
      remarkId: props.data.remarkId,
      busy: false,
      error: false,
      status: ''
    };

    this.onSaveClick = this.onSaveClick.bind(this);
    this.onDeleteClick = this.onDeleteClick.bind(this);
    this.onRemarkChange = this.onRemarkChange.bind(this);
  }

  onSaveClick = () => {
    this.showBusy();

    const task = this.props.new
      ? ApiClient.addRemark({
          latitude: this.props.data.latitude,
          longitude: this.props.data.longitude,
          remark: this.state.remark
        })
      : ApiClient.updateRemark(this.state.remarkId, this.state.remark);

    task.then(res => {
      if(this.props.new) {
        const data = res.data.data;
        this.props.onAction(data, 'insert');
      } else {
        const data = this.props.data;
        data.remark = this.state.remark;
        this.props.onAction(data, 'update');
      }
    })
    .catch(error => {
      this.showError(error.message);
    });
  }

  onDeleteClick = () => {
    this.showBusy();

    ApiClient.deleteRemark(this.state.remarkId)
      .then(res => {
        const data = this.props.data;
        this.props.onAction(data, 'delete');
      })
      .catch(error => {
        this.showError(error.message);
      });
  }

  onRemarkChange = (event) => {
    this.setState({remark: event.target.value});
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
      status: message
    });
  }

  isReadonly() {
    return !this.props.editable && !this.props.new;
  }

  showStatus() {
    return this.state.busy || this.state.error;
  }

  render() {
    return (
      <Wrapper>
        <Header>{this.props.data.email}:</Header>
        {this.isReadonly()
          ? (<ReadOnly>{this.props.data.remark}</ReadOnly>)
          : (<Input value={this.state.remark} onChange={this.onRemarkChange} />)}

        <Footer show={this.showStatus()} error={this.state.error}>{this.state.status}</Footer>

        {!this.isReadonly() && (
          <ButtonWrapper>
            <Button onClick={this.onSaveClick} disabled={this.state.busy}>Save</Button>
            {!this.props.new && (<Button onClick={this.onDeleteClick} disabled={this.state.busy}>Delete</Button>)}
          </ButtonWrapper>
        )}
      </Wrapper>
    );
  }
}

export default RemarkComponent;