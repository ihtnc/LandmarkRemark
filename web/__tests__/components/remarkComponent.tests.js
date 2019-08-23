import React from 'react';
import { shallow } from 'enzyme';
import Remark from "@components/remarkComponent";

describe("RemarkComponent", () => {
  let data = {};

  beforeEach(() => {
    data = {
      remarkId: 'remarkId',
      email: 'email@address',
      latitude: 123,
      longitude: 789,
      remark: 'remark'
    };
  });

  test("should render correctly when editable", () => {
    const wrapper = shallow(<Remark data={data} editable={true} />)
    expect(wrapper).toMatchSnapshot();
  });

  test("should render correctly when new", () => {
    const wrapper = shallow(<Remark data={data} editable={true} new={true} />)
    expect(wrapper).toMatchSnapshot();
  });

  test("should render correctly when not editable", () => {
    const wrapper = shallow(<Remark data={data} />)
    expect(wrapper).toMatchSnapshot();
  });
});