using Sciendo.Music.Contracts.MusicService;

namespace Sciendo.Music.DataProviders.Models
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
