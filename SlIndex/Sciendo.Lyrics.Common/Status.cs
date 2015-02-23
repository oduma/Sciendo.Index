using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Lyrics.Common
{
    public enum Status
    {
        NotStarted,
        FileNotFound,
        FileNotTagged,
        ArtistSongRetrievedFromFile,
        LyricsUrlUnreachable,
        LyricsNotFound,
        LyricsDownloadedOk,
        NotIndexed,
        Done
    }
}
