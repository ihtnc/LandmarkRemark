import React, {Component} from 'react';
import PropTypes from 'prop-types';
import { Map, InfoWindow, Marker, GoogleApiWrapper } from 'google-maps-react';
import Constants from '@src/constants'

class LandmarkMap extends Component {
  state = {
    activeMarker: {}
  };

  static propTypes = {
    center: PropTypes.object,
    zoom: PropTypes.number
  }

  static defaultProps = {
    center: { lat: -33.856762, lng: 151.215295 }, //Sydney Opera House
    zoom: 17
  }

  onMarkerClick = (props, marker, e) =>
    this.setState({
      activeMarker: marker
    });

  onMapClick = (props) => {
    if (this.state.activeMarker != null) {
      this.setState({
        activeMarker: null
      })
    }
  };

  onInfoWindowClose = (props) => {
    if (this.state.activeMarker != null) {
      this.setState({
        activeMarker: null
      })
    }
  };

  constructor(props = defaultProps) {
    super(props);
  }

  render() {
    return (
      <Map
        google={this.props.google}
        initialCenter={this.props.center}
        zoom={this.props.zoom}
        style={{ height: '100%', position: 'relative', width: '100%' }}
        onClick={this.onMapClick}
      >
        <Marker
          name={'Current location'}
          onClick={this.onMarkerClick}
        />

        <InfoWindow
          marker={this.state.activeMarker}
          visible={this.state.activeMarker != null}
          onClose={this.onInfoWindowClose}
        >
          <div>test</div>
        </InfoWindow>
      </Map>
    );
  }
}

export default GoogleApiWrapper({
  apiKey: (Constants.GOOGLE_MAP_API_KEY)
})(LandmarkMap);