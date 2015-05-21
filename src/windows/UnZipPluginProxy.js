var cordova = require('cordova'),
    UnZipPlugin= require('./UnZipPlugin');

module.exports = {

    UnZip: function (successCallback, errorCallback, strInput) {

        var upperCase = strInput[0].toUpperCase();
        if(upperCase != "") {
            successCallback(upperCase);
        }
        else {
            errorCallback(upperCase);
        }
    }
};

require("cordova/exec/proxy").add("UnZipPlugin", module.exports);