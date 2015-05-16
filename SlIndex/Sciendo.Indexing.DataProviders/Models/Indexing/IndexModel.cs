
namespace Sciendo.Music.DataProviders.Models.Indexing
{
    public class IndexModel
    {
        public string SourceFolder { get; set; }

        public IndexModel(string sourceFolder)
        {
            SourceFolder = sourceFolder;
        }
    }
}
