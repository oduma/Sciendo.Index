using Sciendo.Index.Web.IndexingClient;

namespace Sciendo.Index.Web.Models
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
