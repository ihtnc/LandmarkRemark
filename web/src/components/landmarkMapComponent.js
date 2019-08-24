import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import { Map, Marker, GoogleApiWrapper } from 'google-maps-react';

import config from '@src/config';
import session from '@src/session';

import Remark from './remarkComponent';
import InterActiveInfoWindow from './interactiveInfoWindow';

class LandmarkMapComponent extends PureComponent {
  state = {
    activeMarker: {},
    newMarker: null
  };

  static propTypes = {
    center: PropTypes.object,
    zoom: PropTypes.number,
    remarks: PropTypes.array
  }

  static defaultProps = {
    center: { lat: -33.856762, lng: 151.215295 }, //Sydney Opera House
    zoom: 17,
    remarks: null
  }

  constructor(props) {
    super(props);
  }

  onMarkerClick = (props, marker, e) => {
    // reset previous active marker
    if(this.state.activeMarker != null) {
      this.setState({ activeMarker: null });
    }

    // trying to select a marker with a new marker added
    if(marker.data.remarkId && this.state.newMarker != null) {
      this.setState({ newMarker: null });
    }

    // set marker as active
    this.setState({
      activeMarker: marker
    });
  }

  onMapClick = (ref, map, ev) => {
    // reset any new marker added and any previous active markers
    this.setState({ newMarker: null });

    if (this.state.activeMarker != null) {
      this.setState({ activeMarker: null });
    }
  };

  onMapDblClick = (ref, map, ev) => {
    if(!this.props.remarks) { return; }

    const user = session.getUser();

    // initialise a new marker
    this.setState({
      activeMarker: null,
      newMarker: {
        data: {
          latitude: ev.latLng.lat(),
          longitude: ev.latLng.lng(),
          email: user.email
        },
        position: ev.latLng
      }
    });
  };

  onInfoWindowClose = () => {
    // if there is an active marker, reset
    if (this.state.activeMarker != null) {
      this.setState({
        activeMarker: null,
        newMarker: null
      })
    }
  };

  onRemarkAction = (data, action) => {
    if(action=='insert') {
      // append newly created remark to props
      this.props.remarks.push(data);
    } else if(action=='delete') {
      // remove deleted remark from state
      const index = this.props.remarks.indexOf(data);
      this.props.remarks.splice(index, 1);
    }
    // updated remark already gets updated in the state so no further action needed here

    // reset markers
    this.setState({
      newMarker: null,
      activeMarker: null
    });
  };

  getMarkerIcon(remarkId, email) {
    const newlyAdded = !this.state.activeMarker && !remarkId;
    if(newlyAdded) { return 'http://maps.google.com/mapfiles/ms/icons/yellow-dot.png'; }

    const isActive = this.state.activeMarker && this.state.activeMarker.data && this.state.activeMarker.data.remarkId == remarkId;
    if(isActive) { return 'http://maps.google.com/mapfiles/ms/icons/green-dot.png'; }

    const user = session.getUser();
    const isAuthor = user.email == email;
    if(isAuthor) { return 'http://maps.google.com/mapfiles/ms/icons/red-dot.png'; }

    return 'http://maps.google.com/mapfiles/ms/icons/blue-dot.png';
  }

  renderMarkers(list) {
    return list && list.map(r => (
      <Marker
        key={r.remarkId}
        name={r.remarkId}
        title={`${r.email}: ${r.remark}`}
        onClick={this.onMarkerClick}
        position={{ lat: r.latitude, lng: r.longitude }}
        data={r}
        icon={this.getMarkerIcon(r.remarkId, r.email)}
      />
    ))
  }

  renderNewMarker() {
    if(!this.props.remarks) { return; }
    if(!this.state.newMarker) { return; }

    return (
      <Marker
        name={'New Remark'}
        title={`New Remark`}
        onClick={this.onMarkerClick}
        position={this.state.newMarker.position}
        data={this.state.newMarker.data}
        icon={this.getMarkerIcon()}
      />
    );
  }

  render() {
    const user = session.getUser();
    const list = this.props.remarks;

    return (
      <Map
        centerAroundCurrentLocation
        google={this.props.google}
        zoom={this.props.zoom}
        initialCenter={this.props.center}
        style={{ height: '100%', position: 'relative', width: '100%' }}
        onClick={this.onMapClick}
        onDblclick={this.onMapDblClick}
        disableDoubleClickZoom={true}
      >

        {this.renderMarkers(list)}
        {this.renderNewMarker()}

        <InterActiveInfoWindow
          marker={this.state.activeMarker}
          visible={this.state.activeMarker != null}
          onClose={this.onInfoWindowClose}
        >
          {this.state.activeMarker == null || this.state.activeMarker.data == null
            ? (<div>Please select a marker.</div>)
            : (<Remark
                data={this.state.activeMarker.data}
                editable={this.state.activeMarker.data.email==user.email}
                new={this.state.newMarker != null}
                onAction={this.onRemarkAction}
              />)}
        </InterActiveInfoWindow>

      </Map>
    );
  }
}

export default GoogleApiWrapper({
  apiKey: (config.GOOGLE_MAP_API_KEY)
})(LandmarkMapComponent);