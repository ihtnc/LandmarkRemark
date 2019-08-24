const merge = require("webpack-merge");

const parts = require("./webpack.parts");

module.exports = (env, argv) => {
  const config = (argv.mode=='production') ? parts.prodConfig : parts.devConfig;

  return merge([
    parts.commonConfig,
    config,
    parts.devServer({
      host: process.env.HOST,
      port: process.env.PORT,
    }),
  ]);;
}