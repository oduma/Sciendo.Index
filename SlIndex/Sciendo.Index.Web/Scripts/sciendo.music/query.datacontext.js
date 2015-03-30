function datacontext() {

    var self = this;
    self.filterByFacet = function (query, resultObservable, errorObservable, filterFieldNameObservable, selectedFacetObservable, facetFilteredObservable,pageInfoObservable) {
        return ajaxRequest("get", solrFilterUrl(query(),pageInfoObservable(),filterFieldNameObservable(), selectedFacetObservable()))
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            displayResults(data, resultObservable, errorObservable, facetFilteredObservable, pageInfoObservable, true);
        }
        function getFailed() {
            errorObservable("Error retrieving results.");
            resultObservable();
            facetFilteredObservable(false);

        }
    }
    self.doSearch = function (query, resultObservable, errorObservable, facetFilteredObservable, pageInfoObservable) {
        return ajaxRequest("get", solrUrl(query(),pageInfoObservable()))
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            displayResults(data, resultObservable, errorObservable, facetFilteredObservable, pageInfoObservable,false);
        }

        function getFailed() {
            errorObservable("Error retrieving results.");
            resultObservable();
            facetFilteredObservable(false);

        }
    }

    self.addToQueue = function (filePath) {
        return ajaxRequest("get", playerUrl(filePath))
        .done(getSucceeded)
        .fail(getFailed);

        function getSucceeded(data) {
        }
        function getFailed() {
        }
    }

    function displayResults(data,resultObservable, errorObservable, facetFilteredObservable, pageInfoObservable, filtered)
    {
        var grdModel = new ko.simpleGrid.viewModel({
            data: data.ResultRows,
            keyColumn: "FilePath",
            columns: [
                { headerText: "Add to playlist", rowText: "FilePath", isKey:true, isLink:false, colWidth:"0px"},
                { headerText: "Album", rowText: "Album", isKey: false, isLink: false, colWidth: "300px" },
                { headerText: "Artist", rowText: "Artist", isKey: false, isLink: false, colWidth: "100px" },
                { headerText: "Title", rowText: "Title", isKey: false, isLink: true, colWidth: "300px" },
                { headerText: "Lyrics", rowText: "Lyrics", isKey: false, isLink: false, colWidth: "500px" }
            ]
        },self);
        resultObservable({ fields: data.Fields, resultRows: data.ResultRows, gridViewModel: grdModel });

        errorObservable("");

        facetFilteredObservable(filtered);

        pageInfoObservable(data.PageInfo)

    }

    function createResultRow(data) {
        return new datacontext.resultRow(data); // todoItem is injected by todo.model.js
    }


    // Private
    function clearErrorMessage(entity) { entity.errorMessage(null); }
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
    // routes
    function solrUrl(id, pageInfo)
    {
        return "/home/search?criteria=" + (id || "") + "&numRows=" + (pageInfo.RowsPerPage || "0") + "&startRow=" + (pageInfo.PageStartRow || "0");
    }

    function solrFilterUrl(id, pageInfo, facetName, facetId) {
        return "/home/filter?criteria=" + (id || "") + "&numRows=" + (pageInfo.RowsPerPage || "0") + "&startRow=" + (pageInfo.PageStartRow || "0") + "&facetFieldName=" + (facetName || "") + "&facetFieldValue=" + (facetId || "");
    }

    function playerUrl(filePath)
    {
        return "/home/addsongtoqueue?filePath=" + (filePath || "");
    }
};