using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.DataProviders.Models.Playlist
{
    public class PlaylistModel
    {
        public List<PlaylistPageModel> Pages { get; set; }

        public PlaylistModel()
        {
            Pages = new List<PlaylistPageModel>();
        }
    }
}
