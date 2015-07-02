using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Contracts.Analysis
{
    public class StatisticRow
    {
        public string Name { get; set; }

        public int Files { get; set; }

        public int MusicFiles { get; set; }

        public int Tagged { get; set; }

        public int NotTagged { get; set; }

        public int AlbumTag { get; set; }

        public int ArtistTag { get; set; }

        public int TitleTag { get; set; }

        public int Lyrics { get; set; }

        public int LyricsOk { get; set; }

        public int LyricsError { get; set; }

        public int LyricsNotFound { get; set; }

        public int Indexed { get; set; }

        public int NotIndexed { get; set; }

        public int IndexedArtist { get; set; }

        public int IndexedAlbum { get; set; }

        public int IndexedTitle { get; set; }

        public int IndexedLyrics { get; set; }
    }
}
