using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Lyrics.Common
{
    public class LyricsResult:ArtistSong
    {
        
        public string lyrics { get; set; }
        public string url { get; set; }
        public string page_namespace { get; set; }
        public string page_id { get; set; }
        public string isOnTakedownList { get; set; }
    }
}
