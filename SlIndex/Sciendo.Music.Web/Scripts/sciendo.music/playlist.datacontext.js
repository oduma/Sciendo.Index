function pldatacontext() {

    var self = this;

    self.createPlaylist = function (errorObservable) {
        return ajaxRequest("get", createPlaylistUrl())
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded() {
        }
        function getFailed() {
            errorObservable("Error retrieving results.");
        }
    }

    self.getPlaylistPage = function (resultObservable, errorObservable, user, pageInfoObservable) {
        return ajaxRequest("get", getPlaylistPageUrl(user(),pageInfoObservable()))
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            self.displayResults(data, resultObservable, errorObservable, pageInfoObservable);
        }
        function getFailed() {
            errorObservable("Error retrieving results.");
            resultObservable();

        }
    }

    self.refreshPlaylist = function (user, resultObservable, errorObservable, pageInfoObservable) {
        return ajaxRequest("get", getRefreshUrl(user()))
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            
            self.displayResults(data, resultObservable, errorObservable, pageInfoObservable);
        }

        function getFailed() {
            errorObservable("Error retrieving results.");
            resultObservable();

        }
    }

    self.changePlaylist = function (playlistItem, onOff, resultObservable, errorObservable, pageInfoObservable) {
        return ajaxRequest("get", changeItemInPlaylistUrl(playlistItem, onOff, pageInfoObservable()))
        .done(getSucceeded)
        .fail(getFailed);

        function getSucceeded(data) {
            self.displayResults(data, resultObservable, errorObservable, pageInfoObservable);
        }
        function getFailed() {
            errorObservable("Error retrieving results.");
            resultObservable();
        }
    }

    self.displayResults=function(data, resultObservable, errorObservable, pageInfoObservable) {

        for (var i = 0; i < data.ResultRows.length; i++)
        {
            data.ResultRows[i].select = function (itemId, e) {
                var element = e.srcElement;

                if (e.srcElement == null)
                    element = e.originalEvent.srcElement;

                self.changePlaylist(element.id, element.checked, resultObservable, errorObservable,pageInfoObservable);

            };
        }
        var grdModel = new ko.simpleGrid.viewModel({
            data: data.ResultRows,
            keyColumn: "FilePath",
            columns: [
                { headerText: "File Path", rowText: "FilePath", isKey: true, isLink: false, isSelect: false, colWidth: "0px" },
                { headerText: "Add to playlist", rowText: "Included", isKey: false, isLink: false, isSelect: true, colWidth: "50px" },
                { headerText: "Album", rowText: "Album", isKey: false, isLink: false, isSelect: false, colWidth: "300px" },
                { headerText: "Artist", rowText: "Artist", isKey: false, isLink: false, isSelect: false, colWidth: "100px" },
                { headerText: "Title", rowText: "Title", isKey: false, isLink: false, isSelect: false, colWidth: "300px" },
                { headerText: "Lyrics", rowText: "Lyrics", isKey: false, isLink: false, isSelect: false, colWidth: "500px" }
            ]
        }, self);
        resultObservable({ fields: data.Fields, resultRows: data.ResultRows, gridViewModel: grdModel });

        errorObservable("");

        pageInfoObservable(data.PageInfo);

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
    function getRefreshUrl(userName) {
        return config.contextPath + "query/refreshplaylist?lastFmUserName=" + (userName || "");
    }

    function getPlaylistPageUrl(userName, pageInfo) {
        return config.contextPath + "query/getplaylistpage?lastFmUserName=" + (userName || "") +"&numRows=" + (pageInfo.RowsPerPage || "0") + "&startRow=" + (pageInfo.PageStartRow || "0");
    }

    function changeItemInPlaylistUrl(playlistItem,onOff,pageInfo) {
        return config.contextPath + "query/changeiteminplaylist?playlistItem=" + (playlistItem || "") + "&onOff=" + (onOff || false) + "&numRows=" + (pageInfo.RowsPerPage || "0") + "&startRow=" + (pageInfo.PageStartRow || "0");
    }

    function createPlaylistUrl() {
        return config.contextPath + "query/createplaylist";
    }
};