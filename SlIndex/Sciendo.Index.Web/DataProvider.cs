using Sciendo.Index.Web.IndexingClient;

namespace Sciendo.Index.Web
{
    public class DataProvider:IDataProvider
    {
        IIndexerAgent _svc = new IndexerAgentClient();

        public string[] GetMuiscAutocomplete(string term)
        {
            
            return _svc.ListAvailableMusicPathsForIndexing(term);
        }

        public string[] GetLyricsAutocomplete(string term)
        {
            return _svc.ListAvailableLyricsPathsForIndexing(term);
        }

        public SourceFolders GetSourceFolders()
        {
            return _svc.GetSourceFolders();
        }
    }
}
