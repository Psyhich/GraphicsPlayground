module.exports = {
    entry: './public/editor.mjs',
    output: {
        path: __dirname + '/public',
        filename: 'editor.bundle.js'
    },
    optimization: {
        minimize: false
    },
};