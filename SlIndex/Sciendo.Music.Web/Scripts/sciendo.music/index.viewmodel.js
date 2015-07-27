function indexViewModel(musicSource) {
    var self = this;
    self.indexFromPath = ko.observable(replaceAll(musicSource, "/", "\\"));
    self.actions = ko.observableArray(["Acquire and Index", "Acquire only no retry", "Acquire only retry existing"]);
    self.selectedAction = ko.observable("Choose an action ...");
    self.indexingResult = ko.observable();
    self.indexingError = ko.observable();
    self.actionSelected = function (event, data) {
        if (self.selectedAction() == "Acquire and Index") {
            self.index();
            return;
        }
        else if(self.selectedAction()=="Acquire only no retry")
        {
            self.acquireLyrics(false);
            return;
        }
        else if (self.selectedAction()=="Acquire only retry existing")
        {
            self.acquireLyrics(true);
            return;
        }
    }
    self.indexedOccured = ko.computed(function() {
         return self.indexingResult() != null || self.indexingError() != null;
    });
    self.indexedWithError = ko.computed(function() { return self.indexingError() != null; });
    self.indexingResultMessage = ko.computed(function () {
        if (!self.indexedWithError()) {
            if (self.indexingResult() != "")
                return self.indexingResult() + " files Ok.";
            else
                return "";
        }
        return "Error during last operation:";
    });

    self.asyncOperationsHub = $.connection.asyncOperationsHub;

    self.selectValue = function (property, value) {
            self.indexFromPath(value);
    }
    self.index = function () {
        self.indexingError(null);
        self.indexingResult("");
        if ($.connection.hub.state != $.connection.connectionState.connected) {
            $.connection.hub.start().done(function () {
                self.asyncOperationsHub.server.startIndexing(self.indexFromPath());
            });
        }
        else {
            self.asyncOperationsHub.server.startIndexing(self.indexFromPath());
        }

    }

    self.acquireLyrics = function (retry) {
        self.indexingError(null);
        self.indexingResult("");

        if ($.connection.hub.state != $.connection.connectionState.connected) {
            $.connection.hub.start().done(function () {
                self.asyncOperationsHub.server.startAcquiringLyrics(self.indexFromPath(), retry);
            });
        }
        else {
            self.asyncOperationsHub.server.startAcquiringLyrics(self.indexFromPath(), retry);
        }

    }

    self.asyncOperationsHub.client.returnCompletedMessage = function (data)
    {
        self.indexingResult("Processed: " + data.NumberOfDocuments);

        self.indexingError(data.Error);
    }

}

function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}
function replaceAll(string, find, replace) {
    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}