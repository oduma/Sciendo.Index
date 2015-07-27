function statisticsViewModel(model) {
    var self = this;
    self.snapshots = ko.observableArray(model);
    self.error = ko.observable();
    self.resultData = ko.observable();
    self.noDataCollection = ko.observable(true);
    self.newSnapshotName = ko.observable("");
    self.selectedSnapshotId = ko.observable("");
    self.canSave = ko.computed(function () {
        if (self.newSnapshotName() == "")
            return "disabled";
        else
            return "";
    });

    self.takeNew = function (itemId, e)
    {
        self.noDataCollection(false);
    }

    self.save = function (itemId, e)
    {
        if (self.newSnapshotName() != "")
        {
            self.noDataCollection(true);

            ajaxRequest("get", self.takeNewSnapshotUrl())
                .done(getSuccess)
                .fail(getFailed);
            function getSuccess(data) {
                self.snapshots.push(data);
                self.error("");
            }
            function getFailed() {
                self.error("Error taking new snapshot.");
            }
        }
    }

    self.cancel=function(itemId, e)
    {
        self.noDataCollection(true);
    }

    self.parseDate = function (jsonDate)
    {
        return new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""),10));
    }

    self.getSnapshotDetails = function (itemId, e) {
        var element = e.srcElement;

        if (e.srcElement == null)
            element = e.originalEvent.srcElement;
        var id = (element.id != "") ? element.id : element.parentElement.id;

        self.getDetails(self.snapshotUrl(id),id);
    };

    self.getDetails=function(url, id)
    {

        return ajaxRequest("get", url)
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            self.selectedSnapshotId(id);
            self.displayResults(data);
        }
        function getFailed() {
            self.error("Error retrieving results.");
        }

    }
    self.snapshotUrl= function(id) {
        return config.contextPath + "statistics/getSnapshotDetails?id=" + (id || "");
    }

    self.takeNewSnapshotUrl=function()
    {
        return config.contextPath + "statistics/takeNew?name=" + self.newSnapshotName();
    }
    self.displayResults =function(data) {
        for (i = 0; i < data.length ; i++)
        {
            data[i].Id = data[i].Name;
            if (data[i].Tagged == 0)
                data[i].TaggedWithDetails = 0;
            else
                data[i].TaggedWithDetails = "<label>" + data[i].Tagged + "</label></br>Album:&nbsp;" + data[i].AlbumTag + "</br>Artist:&nbsp;" + data[i].ArtistTag + "</br>Title:&nbsp;" + data[i].TitleTag + "</br>";
            if (data[i].Lyrics == 0)
                data[i].LyricsWithDetails = 0;
            else
                data[i].LyricsWithDetails = "<label>" + data[i].Lyrics + "</label></br>Ok:&nbsp;" + data[i].LyricsOk + "</br>Error:&nbsp;" + data[i].LyricsError + "</br>";
            if (data[i].Indexed == 0)
                data[i].IndexedWithDetails = 0;
            else
                data[i].IndexedWithDetails = "<label>" + data[i].Indexed + "</label></br>Album:&nbsp;" + data[i].IndexedAlbum + "</br>Artist:&nbsp;" + data[i].IndexedArtist + "</br>Title:&nbsp;" + data[i].IndexedTitle + "</br>Lyrics:&nbsp;" + data[i].IndexedLyrics + "</br>";
        }
        var grdModel = new ko.simpleGrid.viewModel({
            data: data,
            keyColumn: "Id",
            columns: [
                { headerText: "Id", rowText: "Id", isKey: true, isSelect: false, isLink: false, colWidth: "0px" },
                { headerText: "Name", rowText: "Name", isKey: false, isSelect: false, isLink: true, colWidth: "300px" },
                { headerText: "Files", rowText: "Files", isKey: false, isLink: false, isSelect: false, colWidth: "50px" },
                { headerText: "Music Files", rowText: "MusicFiles", isKey: false, isLink: false, isSelect: false, colWidth: "80px" },
                { headerText: "Tag", rowText: "TaggedWithDetails", isKey: false, isLink: false, isSelect: false, colWidth: "50px" },
                { headerText: "No Tag", rowText: "NotTagged", isKey: false, isLink: false, isSelect: false, colWidth: "50px" },
                { headerText: "Lyrics", rowText: "LyricsWithDetails", isKey: false, isLink: false, isSelect: false, colWidth: "50px" },
                { headerText: "No Lyrics", rowText: "LyricsNotFound", isKey: false, isLink: false, isSelect: false, colWidth: "80px" },
                { headerText: "Not Indexed", rowText: "NotIndexed", isKey: false, isLink: false, isSelect: false, colWidth: "80px" },
                { headerText: "Indexed", rowText: "IndexedWithDetails", isKey: false, isLink: false, isSelect: false, colWidth: "50px" }]
        }, self.drillDown);

        self.resultData({ resultRows: data, gridViewModel: grdModel })
        self.error("");
    }
    self.getFolderUrl = function (folderName, snapshotId) {
        return config.contextPath + "statistics/getFolderDetails?folderName=" + (folderName || "") + "&id="+(snapshotId() || 0);
    }

    self.drillDown = function (itemId, e) {
        var element = e.srcElement;

        if (e.srcElement == null)
            element = e.originalEvent.srcElement;

        var id = (element.id != "") ? element.id : element.parentElement.id;

        self.getDetails(self.getFolderUrl(id,self.selectedSnapshotId),self.selectedSnapshotId());
    }
}