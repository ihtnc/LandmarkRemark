import React from 'react';
import { shallow } from 'enzyme';
import {
  Wrapper,
  Header,
  Status,
  Label,
  Input,
  PasswordStrengthIcon,
  ButtonWrapper,
  Button
} from "@styles/controls";

describe("Controls styled components", () => {
  test("Wrapper should match latest snapshot", () => {
    const wrapper = shallow(<Wrapper />)
    expect(wrapper).toMatchSnapshot();
  });

  test("Header should match latest snapshot", () => {
    const wrapper = shallow(<Header />)
    expect(wrapper).toMatchSnapshot();
  });

  describe("Status", () => {
    test("should render correctly for error", () => {
      const wrapper = shallow(<Status error={true} />);
      expect(wrapper).toMatchSnapshot();
    });

    test("should render correctly for non error", () => {
      const wrapper = shallow(<Status />);
      expect(wrapper).toMatchSnapshot();
    });
  });

  test("Label should match latest snapshot", () => {
    const wrapper = shallow(<Label />)
    expect(wrapper).toMatchSnapshot();
  });

  test("Input should match latest snapshot", () => {
    const wrapper = shallow(<Input />)
    expect(wrapper).toMatchSnapshot();
  });

  test("PasswordStrengthIcon should match latest snapshot", () => {
    const wrapper = shallow(<PasswordStrengthIcon />)
    expect(wrapper).toMatchSnapshot();
  });

  test("ButtonWrapper should match latest snapshot", () => {
    const wrapper = shallow(<ButtonWrapper />)
    expect(wrapper).toMatchSnapshot();
  });

  test("Button should match latest snapshot", () => {
    const wrapper = shallow(<Button />)
    expect(wrapper).toMatchSnapshot();
  });
});