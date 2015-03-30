using Sciendo.Music.DataProviders.MusicClient;

namespace Sciendo.Music.DataProviders.Models.Indexing
{
    public class IndexModel
    {
        public SourceFolders SourceFolders { get; set; }

        public IndexModel(SourceFolders sourceFolders)
        {
            SourceFolders = sourceFolders;
        }
    }
}
