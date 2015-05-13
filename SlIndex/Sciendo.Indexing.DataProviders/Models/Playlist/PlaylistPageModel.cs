using Sciendo.Music.DataProviders.Models.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Music.DataProviders.Models.Playlist
{
    public class PlaylistPageModel
    {
        public PlaylistItem[] ResultRows { get; set; }
        public PageInfo PageInfo { get; set; }

    }
}
