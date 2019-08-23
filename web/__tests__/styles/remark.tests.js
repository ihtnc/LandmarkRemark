import React from 'react';
import { shallow } from 'enzyme';
import {
  RemarkWrapper,
  RemarkHeader,
  RemarkStatus,
  RemarkLabel,
  RemarkInput,
  RemarkButtonWrapper,
  RemarkButton
} from "@styles/remark";

describe("Remark styled components", () => {
  test("RemarkWrapper should match latest snapshot", () => {
    const wrapper = shallow(<RemarkWrapper />)
    expect(wrapper).toMatchSnapshot();
  });

  test("RemarkHeader should match latest snapshot", () => {
    const wrapper = shallow(<RemarkHeader />)
    expect(wrapper).toMatchSnapshot();
  });

  describe("RemarkStatus", () => {
    test("should render correctly for error", () => {
      const wrapper = shallow(<RemarkStatus error={true} />);
      expect(wrapper).toMatchSnapshot();
    });

    test("should render correctly for non error", () => {
      const wrapper = shallow(<RemarkStatus />);
      expect(wrapper).toMatchSnapshot();
    });
  });

  test("RemarkLabel should match latest snapshot", () => {
    const wrapper = shallow(<RemarkLabel />)
    expect(wrapper).toMatchSnapshot();
  });

  test("RemarkInput should match latest snapshot", () => {
    const wrapper = shallow(<RemarkInput />)
    expect(wrapper).toMatchSnapshot();
  });

  test("RemarkButtonWrapper should match latest snapshot", () => {
    const wrapper = shallow(<RemarkButtonWrapper />)
    expect(wrapper).toMatchSnapshot();
  });

  test("RemarkButton should match latest snapshot", () => {
    const wrapper = shallow(<RemarkButton />)
    expect(wrapper).toMatchSnapshot();
  });
});