const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
	entry: './src/js/index.js',
	output: {
		filename: 'js/bundle.js',
		path: path.resolve(__dirname, '../wwwroot'),
		publicPath: path.resolve(__dirname, '../wwwroot'),
	},
	module: {
		rules: [
			{
				test: /\.css$/,
				use: ['style-loader', 'css-loader'],
			},
			{
				test: /ace-builds\/src-min-noconflict\/.*$/,
				loader: 'raw-loader',
			},
		],
	},
	plugins: [
		new CopyWebpackPlugin({
			patterns: [
				{ from: './src/css', to: 'css/' },
				{ from: './src/images', to: 'images/' },
			],
		}),
	],
};
