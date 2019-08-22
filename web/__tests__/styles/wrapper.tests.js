import React from 'react';
import { shallow } from 'enzyme';
import Wrapper from "@src/styles/wrapper";

describe("Wrapper styled component", () => {
  test("should match latest snapshot", () => {
    const wrapper = shallow(<Wrapper />)
    expect(wrapper).toMatchSnapshot();
  });
});