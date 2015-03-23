using System;
using System.Globalization;
using System.Linq;
using Sciendo.Music.Contracts.MusicService;
using Sciendo.Music.DataProviders.Models;
using IMusic = Sciendo.Music.DataProviders.MusicClient.IMusic;

namespace Sciendo.Music.DataProviders
{
    public sealed class DataProvider:IDataProvider
    {
        private IMusic _svc = new MusicClient.MusicClient();

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

        public int AquireLyrics(string fromPath, bool retryExisting)
        {
            return _svc.AcquireLyricsOnDemandFor(fromPath, retryExisting);
        }

        public void Dispose()
        {
            _svc = null;
        }
    }
}
