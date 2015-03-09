using Sciendo.Index.Web.IndexingClient;

namespace Sciendo.Index.Web
{
    interface IDataProvider
    {
        string[] GetMuiscAutocomplete(string term);
        string[] GetLyricsAutocomplete(string term);
        SourceFolders GetSourceFolders();
    }
}
