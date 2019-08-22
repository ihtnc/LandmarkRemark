const HtmlWebPackPlugin = require("html-webpack-plugin");

exports.module = {
    rules: [{
      test: /\.(js|jsx)$/,
      exclude: /node_modules/,
      use: {
          loader: "babel-loader"
      }
  }, {
    test: /\.html$/,
    use: [
      {
        loader: "html-loader"
      }
    ]
  }]
};

exports.plugins = [
  new HtmlWebPackPlugin({
    template: "./src/index.html",
    filename: "./index.html"
  })
];

exports.devServer = ({ host, port } = {}) => ({
  devServer: {
    stats: "errors-only",
    host, // Defaults to `localhost`
    port, // Defaults to 8080
    open: true,
    overlay: true,
  },
});