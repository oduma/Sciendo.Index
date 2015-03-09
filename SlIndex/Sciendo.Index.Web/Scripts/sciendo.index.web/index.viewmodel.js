function indexViewModel(musicSource, lyricsSource) {
    var self = this;
    self.musicFromPath = ko.observable(replaceAll(musicSource,"/","\\"));
    self.lyricsFromPath = ko.observable(replaceAll(lyricsSource, "/", "\\"));
}

function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}
function replaceAll(string, find, replace) {
    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}