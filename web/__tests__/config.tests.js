import config from "@src/config";

describe("Config module", () => {
  test("should define GOOGLE_MAP_API_KEY", () => {
    expect(config.GOOGLE_MAP_API_KEY).toEqual("AIzaSyCtIWsUoMKvNvD5CiT9OZNFSM1zK4xgMJU");
  });

  test("should define API_URL", () => {
    expect(config.API_URL).toEqual("https://localhost:5001/api/");
  });
});