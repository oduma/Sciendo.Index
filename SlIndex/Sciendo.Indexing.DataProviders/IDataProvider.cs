using Sciendo.Indexing.DataProviders.IndexerClient;
using Sciendo.Indexing.DataProviders.Models;

namespace Sciendo.Indexing.DataProviders
{
    public interface IDataProvider
    {
        string[] GetMuiscAutocomplete(string term);
        string[] GetLyricsAutocomplete(string term);
        SourceFolders GetSourceFolders();
        IndexingResult StartIndexing(string fromPath, IndexType indexType);
        ProgressStatusModel[] GetMonitoring();
    }
}
