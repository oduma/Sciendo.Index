using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Music.Contracts.Analysis
{
    [Flags]
    public enum LyricsFileFlag
    {
        NoLyricsFile=0,
        HasLyricsFile = 1,
        LyricsFileWithError = 2,
        LyricsFileNoLyrics = 4,
        LyricsFileOk = 8,
    }
}
