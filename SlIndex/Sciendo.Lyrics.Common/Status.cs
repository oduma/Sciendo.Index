﻿using System;
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
        UnknownTagVersion,
        ArtistSongRetrievedFromFile,
        LyricsUrlUnreachable,
        LyricsDownloadedOk,
        NotIndexed,
        Done
    }
}