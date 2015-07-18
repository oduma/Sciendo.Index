function feedbackViewModel(appProxy) {
    var self = this;
    self.appProxy = appProxy;
    self.statusMessage = ko.observable("");
    self.activityDetails = ko.observable("");
    self.analysisStarted = ko.observable(false);
    self.staticText = ko.observable("Status:");

    self.init = function () {
        appProxy.server.getCurrentStatus().done(
            function (message) {
                if (message != "") {
                    self.analysisStarted(true);
                    self.statusMessage(message);
                }
            }).fail(function (a1, a2) {
                alert(a1);
            }
            )
    }

    self.appProxy.client.updateCurrentActivity = function (data) {
        if (data != "") {
            self.analysisStarted(true);
            self.statusMessage(data);
        }
        else {
            self.analysisStarted(false);
            self.statusMessage("");
        }
    }

    self.appProxy.client.updateDetails = function (data) {
        self.activityDetails(data);
    }
}