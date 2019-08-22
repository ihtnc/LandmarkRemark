import React from 'react';
import { shallow } from 'enzyme';
import LandmarkMap from "@src/components/landmarkmap";

describe("LandmarkMap component", () => {
  test("should match latest snapshot", () => {
    const wrapper = shallow(<LandmarkMap />)
    expect(wrapper).toMatchSnapshot();
  });
});