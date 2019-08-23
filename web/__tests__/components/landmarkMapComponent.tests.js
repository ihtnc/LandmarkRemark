import React from 'react';
import { shallow } from 'enzyme';
import LandmarkMap from "@components/landmarkMapComponent";

describe("LandmarkMapComponent", () => {
  test("should match latest snapshot", () => {
    const wrapper = shallow(<LandmarkMap />)
    expect(wrapper).toMatchSnapshot();
  });
});