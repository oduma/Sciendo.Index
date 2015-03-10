using Sciendo.Indexing.DataProviders.IndexerClient;

namespace Sciendo.Indexing.DataProviders.Models
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
