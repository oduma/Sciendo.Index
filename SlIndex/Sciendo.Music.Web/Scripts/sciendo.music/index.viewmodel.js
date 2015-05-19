function indexViewModel(musicSource) {
    var self = this;
    self.indexFromPath = ko.observable(replaceAll(musicSource,"/","\\"));
    self.retryExisting = ko.observable(false);
    self.indexingResult = ko.observable();
    self.indexingError = ko.observable();
    self.acquireLyricsText = ko.observable("Acquire Lyrics");
    self.indexText = ko.observable("Index");
    self.indexedOccured = ko.computed(function() {
         return self.indexingResult() != null || self.indexingError() != null;
    });
    self.indexedWithError = ko.constructor(function() { return self.indexingError() != null; });
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

    //self.monitoringHub = $.connection.monitoringHub;
    //self.monitoringMessages = ko.observableArray([]);
    //self.maximumMonitoringMessagesDisplay = ko.observable(10);
    //self.monitoringActionName = ko.observable("Subscribe");

    //self.toggle = function () {
    //    var on = (self.monitoringActionName() == "Subscribe");
    //    if ($.connection.hub.state != $.connection.connectionState.connected) {
    //        $.connection.hub.start().done(function () {
    //            self.monitoringHub.server.toggleSending(on);
    //        });
    //    }
    //    else {
    //        self.monitoringHub.server.toggleSending(on);
    //    }
    //    if (on) {
    //        self.monitoringActionName("Unsubscribe");
    //        if ($.connection.hub.state != $.connection.connectionState.connected)
    //        {
    //            $.connection.hub.start().done(function () {
    //                self.monitoringHub.server.send();
    //            });
    //        }
    //        else
    //        {
    //            self.monitoringHub.server.send();
    //        }
    //    } else {
    //        self.monitoringActionName("Subscribe");
    //    }
        
    //}
    self.notAcquiring = ko.computed(function () { return self.acquireLyricsText() == "Acquire Lyrics"; });

    self.notIndexing = ko.computed(function () { return self.indexText() == "Index"; });
    //self.hasSubscription = ko.computed(function () { return self.monitoringActionName() == "Unsubscribe"; });

    //self.monitoringHub.client.addNewMessageToPage = function (message) {
    //    if (self.monitoringMessages().indexOf(message) != -1)
    //        alert("mesaj nou");
    //    if (self.monitoringMessages().length > self.maximumMonitoringMessagesDisplay())
    //        self.monitoringMessages.removeAll();
    //    message.StatusOk = (message.Status == 'Done' || message.Status == 'LyricsDownloadedOk') ? true : false;
    //    message.DetailsVisible = ko.observable(false);
    //    message.DetailsActionName = ko.observable("Details");
    //    message.revealDetails = function () {
    //        if (message.DetailsVisible()) {
    //            message.DetailsVisible(false);
    //            message.DetailsActionName("Details");
    //        } else {
    //            message.DetailsVisible(true);
    //            message.DetailsActionName("Hide Details");
    //        }
    //    };
    //    self.monitoringMessages.push(message);
    //}

    self.selectValue = function (property, value) {
            self.indexFromPath(value);
    }
    self.index = function () {
        self.indexText("Indexing...");
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

    self.acquireLyrics = function () {
        self.acquireLyricsText("Acquiring Lyrics...");
        self.indexingError(null);
        self.indexingResult("");

        if ($.connection.hub.state != $.connection.connectionState.connected) {
            $.connection.hub.start().done(function () {
                self.asyncOperationsHub.server.startAcquiringLyrics(self.indexFromPath(), self.retryExisting());
            });
        }
        else {
            self.asyncOperationsHub.server.startAcquiringLyrics(self.indexFromPath(), self.retryExisting());
        }

    }

    self.asyncOperationsHub.client.returnCompletedMessage = function (data)
    {
        var lastActivity = "Indexed ";
        if (self.acquireLyricsText() == "Acquiring Lyrics...") {
            self.acquireLyricsText("Acquire Lyrics");
            lastActivity = "Acquired Lyrics for ";
        }
        else {
            self.indexText("Index");

        }
        self.indexingResult(lastActivity + data.NumberOfDocuments);

        self.indexingError(data.Error);
        //if ($.connection.hub.state != $.connection.connectionState.connected) {
        //    $.connection.hub.start().done(function () {
        //        self.monitoringHub.server.send();
        //    });
        //}
        //else {
        //    self.monitoringHub.server.send();
        //}
    }

}

function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}
function replaceAll(string, find, replace) {
    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}