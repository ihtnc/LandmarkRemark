const merge = require("webpack-merge");
const path = require("path");

const parts = require("./webpack.parts");

var commonConfig = {
  module: parts.module,
  plugins: parts.plugins,
  devtool: 'eval-source-map',
  resolve: {
    alias: {
      "@src": path.resolve(__dirname, "src"),
      "@api": path.resolve(__dirname, "src/api"),
      "@components": path.resolve(__dirname, "src/components"),
      "@styles": path.resolve(__dirname, "src/styles")
    }
  }
};

const config = merge([
  commonConfig,
  parts.devServer({
    host: process.env.HOST,
    port: process.env.PORT,
  }),
]);

module.exports = config;