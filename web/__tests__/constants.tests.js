import Constants from "@src/constants";

describe("Constants", () => {
  test("should define GOOGLE_MAP_API_KEY", () => {
    expect(Constants.GOOGLE_MAP_API_KEY).toEqual("AIzaSyCtIWsUoMKvNvD5CiT9OZNFSM1zK4xgMJU");
  });
});