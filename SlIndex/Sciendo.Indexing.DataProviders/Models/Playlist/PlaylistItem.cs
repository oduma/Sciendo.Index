﻿using Sciendo.Music.Solr.Query.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Music.DataProviders.Models.Playlist
{
    public class PlaylistItem:Doc
    {
        public bool Included { get; set; }
    }
}
