function playlistViewModel() {
    var self = this;
    self.lastFmUser = ko.observable();
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
        (new pldatacontext()).refreshPlaylist(self.lastFmUser, self.resultData, self.error, self.facetFiltered, self.pageInfo);
    }
    self.getNextPage = function () {
        self.pageInfo().PageStartRow += self.pageInfo().RowsPerPage;
            (new datacontext()).getPlaylistPage(self.resultData, self.error, self.facetFiltered, self.pageInfo);
    }
    self.getPreviousPage = function () {
        self.pageInfo().PageStartRow -= self.pageInfo().RowsPerPage;
        if (self.facetFiltered())
            (new datacontext()).filterByFacet(self.criteria, self.resultData, self.error, self.selectedFacetFieldName, self.selectedFacetValue, self.facetFiltered, self.pageInfo);
        else
            (new datacontext()).doSearch(self.criteria, self.resultData, self.error, self.facetFiltered, self.pageInfo);
    }

    self.artistFacetSelected = function () {
        self.selectedFacetFieldName("artist_f");
        self.pageInfo({ TotalRows: 0, RowsPerPage: 0, PageStartRow: 0 });
        (new datacontext()).filterByFacet(self.criteria, self.resultData, self.error, self.selectedFacetFieldName, self.selectedFacetValue, self.facetFiltered, self.pageInfo);
    }
    self.extensionFacetSelected = function () {
        self.selectedFacetFieldName("extension_f");
        self.pageInfo({ TotalRows: 0, RowsPerPage: 0, PageStartRow: 0 });
        (new datacontext()).filterByFacet(self.criteria, self.resultData, self.error, self.selectedFacetFieldName, self.selectedFacetValue, self.facetFiltered, self.pageInfo);
    }
    self.letterFacetSelected = function () {
        self.selectedFacetFieldName("letter_catalog_f");
        self.pageInfo({ TotalRows: 0, RowsPerPage: 0, PageStartRow: 0 });
        (new datacontext()).filterByFacet(self.criteria, self.resultData, self.error, self.selectedFacetFieldName, self.selectedFacetValue, self.facetFiltered, self.pageInfo);
    }
}

// Initiate the Knockout bindings
ko.applyBindings(new playlistViewModel());
