using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Music.Contracts.Analysis
{
    [Flags]
    public enum MusicFileFlag
    {
        NotAMusicFile=0,
        IsMusicFile = 1,
        HasTag = 2,
        HasArtistTag = 4,
        HasAlbumTag = 8,
        HasTitleTag = 16,
    }
}
