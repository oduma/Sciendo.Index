function playlistViewModel(defaultUserName) {
    var self = this;
    self.lastFmUser = ko.observable(defaultUserName);
    self.resultData = ko.observable(),
        self.error = ko.observable(),
        self.pageInfo = ko.observable({ TotalRows: 0, RowsPerPage: 0, PageStartRow: 0 }),
    self.notMaxPage = ko.computed(function () {
        return (self.pageInfo().PageStartRow + self.pageInfo().RowsPerPage) < self.pageInfo().TotalRows;
    }, self);

    self.pageLastIndex = ko.computed(function () {
        return (self.pageInfo().PageStartRow + self.pageInfo().RowsPerPage > self.pageInfo().TotalRows) ? self.pageInfo().TotalRows : self.pageInfo().PageStartRow + self.pageInfo().RowsPerPage;
    }, self);
    self.getResults = function () {
        (new pldatacontext()).refreshPlaylist(self.lastFmUser, self.resultData, self.error,self.pageInfo);
    }
    self.createPlaylist=function() {
        (new pldatacontext()).createPlaylist(self.error);
    }
    self.getNextPage = function () {
        self.pageInfo().PageStartRow += self.pageInfo().RowsPerPage;
        (new pldatacontext()).getPlaylistPage(self.resultData, self.error, self.lastFmUser, self.pageInfo);
    }
    self.getPreviousPage = function () {
        self.pageInfo().PageStartRow -= self.pageInfo().RowsPerPage;
        (new pldatacontext()).getPlaylistPage(self.resultData, self.error, self.lastFmUser, self.pageInfo);
    }
}


