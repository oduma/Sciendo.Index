using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Indexer.Contracts
{
    public interface IIndexerAgent
    {
        void IndexLyricsOnDemand(string fromPath);
        void IndexMusicOnDemand(string fromPath, bool includeLyrics=false );

    }
}
