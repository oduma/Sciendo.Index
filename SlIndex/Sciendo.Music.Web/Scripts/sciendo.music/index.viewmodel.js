function indexViewModel(musicSource) {
    var self = this;
    self.indexFromPath = ko.observable(replaceAll(musicSource,"/","\\"));
    self.retryExisting = ko.observable(false);
    self.indexingResult = ko.observable();
    self.indexingError = ko.observable();
    self.indexedOccured = ko.computed(function() {
         return self.indexingResult() != null || self.indexingError() != null;
    });
    self.indexedWithError = ko.constructor(function() { return self.indexingError() != null; });
    self.indexingResultMessage = ko.computed(function () {
        if (!self.indexedWithError()) {
            return "Indexed " + self.indexingResult() + " files Ok.";
        }
        return "Tryed to index unsuccesfull:";
    });

    self.hub = $.connection.monitoringHub;
    self.subscription = ko.observable("not subscribed");
    self.monitoringMessages = ko.observableArray([]);
    self.maximumMonitoringMessagesDisplay = ko.observable(10);
    self.monitoringActionName = ko.observable("Subscribe");
    //Initializes the view model
    self.toggle = function () {
        var on = (self.monitoringActionName() == "Subscribe");
        self.hub.server.toggleSending(on);
        if (on) {
            self.monitoringActionName("Unsubscribe");
            self.hub.server.send();
        } else {
            self.monitoringActionName("Subscribe");
        }
        
    }
    self.hasSubscription = ko.computed(function () { return self.monitoringActionName() =="Unsubscribe"; });

    self.hub.client.addNewMessageToPage = function (message) {
        if (self.monitoringMessages().length > self.maximumMonitoringMessagesDisplay())
            self.monitoringMessages.removeAll();
        message.StatusOk = (message.Status == 'Done') ? true : false;
        message.DetailsVisible = ko.observable(false);
        message.DetailsActionName = ko.observable("Details");
        message.revealDetails = function () {
            if (message.DetailsVisible()) {
                message.DetailsVisible(false);
                message.DetailsActionName("Details");
            } else {
                message.DetailsVisible(true);
                message.DetailsActionName("Hide Details");
            }
        };
        self.monitoringMessages.push(message);
    }

    self.selectValue = function (property, value) {
            self.indexFromPath(value);
    }
    self.index = function () {
        self.hub.server.startIndexing(self.indexFromPath());
    }

    self.acquireLyrics = function () {
        self.hub.server.startAcquiringLyrics(self.musicFromPath(), self.retryExisting());
    }

    self.hub.client.returnCompletedMessage = function (data)
    {
        self.indexingResult(data.NumberOfDocuments);

        self.indexingError(data.Error);
    }

}

function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}
function replaceAll(string, find, replace) {
    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}