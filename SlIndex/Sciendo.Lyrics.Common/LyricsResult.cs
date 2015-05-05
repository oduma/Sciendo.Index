namespace Sciendo.Lyrics.Common
{
    public class LyricsResult:ArtistSong
    {
        
        public string lyrics { get; set; }
        public string url { get; set; }
        public string page_namespace { get; set; }
        public string page_id { get; set; }
        public string isOnTakedownList { get; set; }
        public override string ToString()
        {
            return
                string.Format(
                    "Artist:{0}, Song:{1}, lyrics:{2}, url:{3}, page namespace:{4}, page id:{5}, Is On Take Dwon List: {6}",
                    (artist) ?? "NoArtist", (song) ?? "NoSong", (lyrics) ?? "NoLyrics", (url) ?? "NoUrl", page_namespace,
                    page_id, isOnTakedownList);
        }
    }
}
