import React from 'react';
import { shallow } from 'enzyme';
import Dashboard from "@components/dashboardComponent";

describe("LandmarkMapComponent", () => {
  test("should render correctly on ready state", () => {
    const email = 'email';

    const wrapper = shallow(<Dashboard email={email} />)
    expect(wrapper).toMatchSnapshot();
  });

  test("should render correctly when there is an error", () => {
    const email = 'email';
    const status = 'message';
    const error = true;

    const wrapper = shallow(<Dashboard email={email} status={status} error={error} />)
    expect(wrapper).toMatchSnapshot();
  });

  test("should render correctly when there is a status update", () => {
    const email = 'email';
    const status = 'message';
    const error = false;

    const wrapper = shallow(<Dashboard email={email} status={status} error={error} />)
    expect(wrapper).toMatchSnapshot();
  });
});