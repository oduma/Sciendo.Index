﻿function ajaxRequest(type, url, data, dataType) { // Ajax helper
    var options = {
        dataType: dataType || "json",
        contentType: "application/json",
        cache: false,
        type: type,
        data: data ? data.toJson() : null
    };
    var antiForgeryToken = $("#antiForgeryToken").val();
    if (antiForgeryToken) {
        options.headers = {
            'RequestVerificationToken': antiForgeryToken
        }
    }
    return $.ajax(url, options);
}
