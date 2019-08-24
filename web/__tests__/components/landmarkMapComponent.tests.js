import React from 'react';
import { shallow } from 'enzyme';
import LandmarkMap from "@components/landmarkMapComponent";

describe("LandmarkMapComponent", () => {
  test("should match latest snapshot", () => {
    const remarks = [];
    const zoom = 18;
    const center = { lat: -33.856762, lng: 151.215295 };

    const wrapper = shallow(<LandmarkMap remarks={remarks} zoom={zoom} center={center} />)
    expect(wrapper).toMatchSnapshot();
  });
});