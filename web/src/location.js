const SYDNEY_OPERA_HOUSE = { lat: -33.856762, lng: 151.215295 };

const getCurrentLocation = (callback) => {
  if(!navigator || !navigator.geolocation || !navigator.geolocation.getCurrentPosition) {
    callback(SYDNEY_OPERA_HOUSE);
    return;
  }

  navigator.geolocation.getCurrentPosition(pos => {
    const current = {
      lat: pos.coords.latitude,
      lng: pos.coords.longitude
    };

    callback(current);
  });
};

export default {
    SYDNEY_OPERA_HOUSE,
    getCurrentLocation
}