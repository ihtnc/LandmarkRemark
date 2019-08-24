import React from 'react';
import { shallow } from 'enzyme';
import AppWrapper from "@styles/appwrapper";

describe("AppWrapper styled component", () => {
  test("should match latest snapshot", () => {
    const wrapper = shallow(<AppWrapper />)
    expect(wrapper).toMatchSnapshot();
  });
});