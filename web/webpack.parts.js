const HtmlWebPackPlugin = require("html-webpack-plugin");
const path = require("path");

exports.commonConfig = {
  output: {
    path: path.resolve(__dirname, '../output/wwwroot')
  },
  devtool: 'eval-source-map',
  resolve: {
    alias: {
      "@src": path.resolve(__dirname, "src"),
      "@api": path.resolve(__dirname, "src/api"),
      "@components": path.resolve(__dirname, "src/components"),
      "@styles": path.resolve(__dirname, "src/styles")
    }
  },
  plugins: [
    new HtmlWebPackPlugin({
      template: "./src/index.html",
      filename: "./index.html"
    })
  ]
};

exports.devConfig = {
  module: {
    rules: [{
      test: /\.(js|jsx)$/,
      exclude: [/node_modules/, /\.prod\.js$/],
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
  },
  resolve: {
    alias: {
      "@config": path.resolve(__dirname, "src/config.dev.js")
    }
  }
};

exports.prodConfig = {
  module: {
    rules: [{
      test: /\.(js|jsx)$/,
      exclude: [/node_modules/, /\.dev\.js$/],
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
  },
  resolve: {
    alias: {
      "@config": path.resolve(__dirname, "src/config.prod.js")
    }
  }
};

exports.devServer = ({ host, port } = {}) => ({
  devServer: {
    stats: "errors-only",
    host, // Defaults to `localhost`
    port, // Defaults to 8080
    open: true,
    overlay: true,
  },
});