using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Music.Contracts.Analysis
{
    [Flags]
    public enum IndexedFlag
    {
        NotIndexed=0,
        Indexed=1,
        IndexedArtist=2,
        IndexedAlbum=4,
        IndexedTitle=8,
        IndexedLyrics=16
    }
}
