using System;
using System.Globalization;
using System.Linq;
using Sciendo.Indexing.DataProviders.IndexerClient;
using Sciendo.Indexing.DataProviders.Models;

namespace Sciendo.Indexing.DataProviders
{
    public sealed class DataProvider:IDataProvider
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
                            NumberOfDocuments = _svc.IndexMusicOnDemand(fromPath).ToString(CultureInfo.InvariantCulture)
                        };
                    case IndexType.Lyrics:
                        return new IndexingResult
                        {
                            IndexType = indexType.ToString(),
                            NumberOfDocuments = _svc.IndexLyricsOnDemand(fromPath).ToString(CultureInfo.InvariantCulture)
                        };
                    default:
                        return new IndexingResult
                        {
                            IndexType = IndexType.None.ToString(),
                            NumberOfDocuments = 0.ToString(CultureInfo.InvariantCulture),
                            Error = "Index Type unknown."
                        };
                }

            }
            catch (Exception ex)
            {
                return new IndexingResult
                {
                    IndexType = indexType.ToString(),
                    NumberOfDocuments = 0.ToString(CultureInfo.InvariantCulture),
                    Error = ex.Message
                };
            }
        }

        public ProgressStatusModel[] GetMonitoring()
        {
            return _svc.GetLastProcessedPackages().Select(p=>new ProgressStatusModel{Id=p.Id.ToString(),Package=p.Package.ToString(CultureInfo.InvariantCulture),Status=p.Status.ToString()}).ToArray();

        }

        public void Dispose()
        {
            _svc = null;
        }
    }
}
