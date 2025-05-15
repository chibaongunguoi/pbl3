function GetData(url, outputComponent = null, inputData = null) {
    var receivedData = null;
    $.get(url, inputData)
        .done(function (data) {
            receivedData = data;
        });

    if (outputComponent != null) {
        $(outputComponent).html(receivedData);
        return;
    }
    else {
        return receivedData;
    }
}