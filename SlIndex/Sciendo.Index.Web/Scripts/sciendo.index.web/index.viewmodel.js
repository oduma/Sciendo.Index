function indexViewModel(musicSource, lyricsSource) {
    var self = this;
    self.musicFromPath = ko.observable(replaceAll(musicSource,"/","\\"));
    self.lyricsFromPath = ko.observable(replaceAll(lyricsSource, "/", "\\"));
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

function indexUrl(id,indexType) {
    return "/home/startIndexing?fromPath=" + (id || "") + "&indexType=" + (indexType || 0);
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