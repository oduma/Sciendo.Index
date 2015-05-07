function indexViewModel(musicSource, lyricsSource) {
    var self = this;
    self.musicFromPath = ko.observable(replaceAll(musicSource,"/","\\"));
    self.lyricsFromPath = ko.observable(replaceAll(lyricsSource, "/", "\\"));
    self.retryExisting = ko.observable(false);
    self.indexingResult = ko.observable();
    self.indexingError = ko.observable();
    self.lastIndexingType = ko.observable();
    self.indexedOccured = ko.computed(function() {
         return self.indexingResult() != null || self.indexingError() != null;
    });
    self.indexedWithError = ko.constructor(function() { return self.indexingError() != null; });
    self.indexingResultMessage = ko.computed(function () {
        if (!self.indexedWithError()) {
            return "Indexed " + self.indexingResult() + " " + self.lastIndexingType() + " files Ok.";
        }
        return "Tryed to index " + self.lastIndexingType() + " unsuccesfull:";
    });

    self.hub = $.connection.monitoringHub;
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
        if (property == "musicIndexFromPath")
            self.musicFromPath(value);
        if (property == "lyricsIndexFromPath")
            self.lyricsFromPath(value);

    }
    self.indexMusic= function() {
        return ajaxRequest("get", indexUrl(self.musicFromPath(),1))
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            displayResults(data, self.indexingResult, self.indexingError, self.lastIndexingType);
        }
        function getFailed() {
            indexingError("Error indexing.");
            indexingResult();
        }
    
    }

    self.acquireLyrics=function() {
        return ajaxRequest("get", acquireLyricsUrl(self.musicFromPath(), self.retryExisting()))
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            displayAcquireLyricsResults(data, self.indexingResult, self.indexingError,self.lastIndexingType);
        }
        function getFailed() {
            indexingError("Error indexing.");
            indexingResult();
        }

    }
    self.indexLyrics= function() {
        return ajaxRequest("get", indexUrl(self.lyricsFromPath(), 2))
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            displayResults(data, self.indexingResult, self.indexingError, self.lastIndexingType);
        }
        function getFailed() {
            indexingError("Error indexing.");
            indexingResult();
        }

    }


}

function displayResults(data, resultObservable, errorObservable, lastIndexingTypeObservable) {

    resultObservable(data.NumberOfDocuments);

    errorObservable(data.Error);

    lastIndexingTypeObservable(data.IndexType);

}

function displayAcquireLyricsResults(data, resultObservable, errorObservable, lastIndexingTypeObservable) {

    resultObservable(data.NumberOfDocuments);

    errorObservable(data.Error);

    lastIndexingTypeObservable("Acquyring Lyrics");

}

function indexUrl(id, indexType) {
    return config.contextPath + "home/startIndexing?fromPath=" + (id || "") + "&indexType=" + (indexType || 0);
}

function acquireLyricsUrl(id, retryExisting) {
    return config.contextPath + "home/startAcquiringLyrics?fromPath=" + (id || "") + "&retryExisting=" + (retryExisting || false);
}

function ajaxRequest(type, url, data, dataType) { // Ajax helper
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
function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}
function replaceAll(string, find, replace) {
    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}