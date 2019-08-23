/*
ISSUE: Children in the google-maps-react/InfoWindow does not have event handlers.
FIX: See reference below

Reference:
https://stackoverflow.com/questions/53615413/how-to-add-a-button-in-infowindow-with-google-maps-react
*/
import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { InfoWindow } from 'google-maps-react';

export default class InteractiveInfoWindow extends Component {
    constructor(props) {
      super(props);
      this.infoWindowRef = React.createRef();
      this.contentElement = document.createElement(`div`);
    }

    componentDidUpdate(prevProps) {
      if (this.props.children !== prevProps.children) {
        ReactDOM.render(
          React.Children.only(this.props.children),
          this.contentElement
        );
        this.infoWindowRef.current.infowindow.setContent(this.contentElement);
      }
    }

    render() {
      return <InfoWindow ref={this.infoWindowRef} {...this.props} />;
    }
  }