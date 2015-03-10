using System;
using System.Linq;
using Sciendo.Indexing.DataProviders.IndexerClient;
using Sciendo.Indexing.DataProviders.Models;

namespace Sciendo.Indexing.DataProviders
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
            var formattedSourceFolders = _svc.GetSourceFolders();
            formattedSourceFolders.Music=formattedSourceFolders.Music.Replace("\\", "/");
            formattedSourceFolders.Lyrics = formattedSourceFolders.Lyrics.Replace("\\", "/");
            return formattedSourceFolders;
        }

        public IndexingResult StartIndexing(string fromPath, IndexType indexType)
        {
            try
            {
                switch (indexType)
                {
                    case IndexType.Music:
                        return new IndexingResult
                        {
                            IndexType = indexType.ToString(),
                            NumberOfDocuments = _svc.IndexMusicOnDemand(fromPath).ToString()
                        };
                    case IndexType.Lyrics:
                        return new IndexingResult
                        {
                            IndexType = indexType.ToString(),
                            NumberOfDocuments = _svc.IndexLyricsOnDemand(fromPath).ToString()
                        };
                    default:
                        return new IndexingResult
                        {
                            IndexType = IndexType.None.ToString(),
                            NumberOfDocuments = 0.ToString(),
                            Error = "Index Type unknown."
                        };
                }

            }
            catch (Exception ex)
            {
                return new IndexingResult
                {
                    IndexType = indexType.ToString(),
                    NumberOfDocuments = 0.ToString(),
                    Error = ex.Message
                };
            }
        }

        public ProgressStatusModel[] GetMonitoring()
        {
            return _svc.GetLastProcessedPackages().Select(p=>new ProgressStatusModel{Id=p.Id.ToString(),Package=p.Package.ToString(),Status=p.Status.ToString()}).ToArray();

        }
    }
}
