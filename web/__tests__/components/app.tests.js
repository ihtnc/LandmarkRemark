import React from 'react';
import { shallow } from 'enzyme';
import App from "@components/app"

describe("App component", () => {
  test("should match latest snapshot", () => {
    const wrapper = shallow(<App />)
    expect(wrapper).toMatchSnapshot();
  });
});