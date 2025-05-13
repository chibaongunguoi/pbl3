function GetData(url, outputComponent = null, inputData = null) {
    var receivedData = null;
    $.get(url, inputData)
        .done(function (data) {
            receivedData = data;
            console.log("success")
        });

    if (outputComponent != null) {
        $(outputComponent).html(receivedData);
        return;
    }
    else {
        return receivedData;
    }
}