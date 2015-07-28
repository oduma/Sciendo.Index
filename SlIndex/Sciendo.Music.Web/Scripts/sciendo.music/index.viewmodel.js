function indexViewModel(musicSource) {
    var self = this;
    self.indexFromPath = ko.observable(replaceAll(musicSource, "/", "\\"));
    self.actions = ko.observableArray(["Acquire and Index", "Acquire only no retry", "Acquire only retry existing"]);
    self.selectedAction = ko.observable("Choose an action ...");
    self.IsIndexing = ko.observable(false);
    self.IsGettingLyrics = ko.observable(false);
    self.indexingError = ko.observable();
    self.indexingReaction = function (indexingOccures) {
        self.IsIndexing(indexingOccures);
    }
    self.getLyricsReaction = function (lyricsAcquisitionInProcess) {
        self.IsGettingLyrics(lyricsAcquisitionInProcess);
    }

    self.isActive = ko.computed(function () {
        return !self.IsIndexing() && !self.IsGettingLyrics();
    });

    self.showAs = ko.computed(function () {
        if (self.isActive())
            return "active";
        else
            return "inactive";
    });
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

    self.selectValue = function (property, value) {
            self.indexFromPath(value);
    }
    self.index = function () {

        return ajaxRequest("get", self.getStartIndexingUrl())
            .fail(getFailed);

        function getFailed() {
            self.indexingError("Error contacting the server.");
        }

    }

    self.getStartIndexingUrl = function () {
        return config.contextPath + "home/startIndexing?fromPath=" + (self.indexFromPath() || "");

    }

    self.acquireLyrics = function (retry) {
        return ajaxRequest("get", self.getStartGetLyricsUrl(retry))
            .fail(getFailed);

        function getFailed() {
            self.indexingError("Error contacting the server.");
        }
    }
    self.getStartGetLyricsUrl = function () {
        return config.contextPath + "home/startAcquireLyrics?fromPath=" + (self.indexFromPath() || "") + "&retry="+retry;

    }

}

function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}
function replaceAll(string, find, replace) {
    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}