function indexViewModel() {
    var self = this;

    self.musicIndexFromPath = ko.observable('').extend({ throttle: 30 });

    self.lyricsIndexFromPath = ko.observable('').extend({ throttle: 30 });
    //item select callback function
    self.selectMusicItemCallback = function (data) {
        //dosomething with the selected item
        console.log(data);
    };
    self.selectLyricsItemCallback = function (data) {
        //dosomething with the selected item
        console.log(data);
    };
}

//apply bindings
ko.applyBindings(new indexViewModel());
