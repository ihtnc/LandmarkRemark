import React from 'react';
import { shallow } from 'enzyme';
import Login from "@components/loginComponent";

describe("LoginComponent", () => {
  test("should match latest snapshot", () => {
    const wrapper = shallow(<Login />)
    expect(wrapper).toMatchSnapshot();
  });
});