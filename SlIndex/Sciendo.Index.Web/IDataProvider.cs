using Sciendo.Index.Web.IndexingClient;
using Sciendo.Index.Web.Models;

namespace Sciendo.Index.Web
{
    interface IDataProvider
    {
        string[] GetMuiscAutocomplete(string term);
        string[] GetLyricsAutocomplete(string term);
        SourceFolders GetSourceFolders();
        IndexingResult StartIndexing(string fromPath, IndexType indexType);
    }
}
