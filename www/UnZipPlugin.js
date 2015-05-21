var UnZipPlugin = {
    UnZip: function (successCallback, errorCallback, strInput) {
        cordova.exec(successCallback, errorCallback, "UnZipPlugin", "UnZip", [strInput]);
    }
}

module.exports = UnZipPlugin;