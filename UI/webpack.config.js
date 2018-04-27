var path = require('path');

//var DIST_DIR =  __dirname + 'D:/ProgrammingProjects/Asound/DmxLedPanels/DmxLedPanel/DmxLedPanel/bin/Debug/UI'
var DIST_DIR = path.resolve(__dirname, "../DmxLedPanel/DmxLedPanel/bin/Debug/UI");
var SRC_DIR = path.resolve(__dirname, "src");

var config = {
    mode: 'production',
    entry: SRC_DIR + '/app/index.js',
    output: {
        path: DIST_DIR + '/resource/app/',
        filename: 'bundle.js',
        publicPath: '/resource/app/'
    },
    module: {
        rules: [
            {
                test: /\.js?/,
                include: SRC_DIR,
                loader: 'babel-loader',
                query: {
                    presets: ['react', 'es2015', 'stage-2']
                }   
            },
            {
                test: /\.css$/,
                include: SRC_DIR,
                use: [ 'css-loader' ]
            }
        ]    
    },
    node: {
        fs: "empty"
     }
}

module.exports = config;