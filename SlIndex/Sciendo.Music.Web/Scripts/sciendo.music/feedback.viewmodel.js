function feedbackViewModel(appProxy) {
    var self = this;
    self.appProxy = appProxy;
    self.analysisStatusMessage = ko.observable("");
    self.analysisActivityDetails = ko.observable("");
    self.indexingStatusMessage = ko.observable("");
    self.indexingActivityDetails = ko.observable("");
    self.playlistStatusMessage = ko.observable("");
    self.playlistActivityDetails = ko.observable("");
    self.playlistStarted = ko.observable(false);
    self.analysisStarted = ko.observable(false);
    self.indexingStarted = ko.observable(false);
    self.started = ko.computed(function () {
        return self.analysisStarted() || self.indexingStarted() || self.playlistStarted();
    });
    self.init = function () {
        appProxy.server.getCurrentAnalysisStatus().done(
            function (message) {
                if (message != "") {
                    self.analysisStarted(true);
                    self.analysisStatusMessage(message);
                }
            }).fail(function (a1, a2) {
                alert(a1);
            }
            );
        appProxy.server.getCurrentIndexingStatus().done(
            function (message) {
                if (message != "") {
                    self.indexingStarted(true);
                    self.indexingStatusMessage(message);
                }

            }
            ).fail(function (a1, a2) {
                alert(a1);
            }
            );
    }

    self.appProxy.client.updateCurrentAnalysisActivity = function (data) {
        if (data != "") {
            self.analysisStarted(true);
            self.analysisStatusMessage(data);
        }
        else {
            self.analysisStarted(false);
        }
    }

    self.appProxy.client.updateAnalysisDetails = function (data) {
        self.analysisActivityDetails(data);
    }

    self.appProxy.client.updateCurrentIndexingActivity = function (data) {
        if (data != "") {
            self.indexingStarted(true);
            self.indexingStatusMessage(data);
        }
        else {
            self.indexingStarted(false);
        }
    }

    self.appProxy.client.updateIndexingDetails = function (data) {
        self.indexingActivityDetails(data);
    }
}