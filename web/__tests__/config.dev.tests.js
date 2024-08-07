import config from "@config";

describe("Config module", () => {
  test("should define GOOGLE_MAP_API_KEY", () => {
    expect(config.GOOGLE_MAP_API_KEY).toEqual("GOOGLE_MAP_API_KEY");
  });

  test("should define API_URL", () => {
    expect(config.API_URL).toEqual("https://localhost:8179/api/");
  });
});