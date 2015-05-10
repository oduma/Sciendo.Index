using System;
using System.Globalization;
using System.Linq;
using Sciendo.Music.DataProviders.Models.Indexing;
using Sciendo.Music.Contracts.MusicService;


namespace Sciendo.Music.DataProviders
{
    public sealed class DataProvider:IDataProvider
    {
        private IMusic _svc = new MusicClient();

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

        public void StartIndexing(string fromPath, IndexType indexType,Action<object,IndexMusicOnDemandCompletedEventArgs> indexMusicCompletedCallback,Action<object,IndexLyricsOnDemandCompletedEventArgs>indexLyricsCompletedCallback)
        {
            ((MusicClient)_svc).IndexMusicOnDemandCompleted += new EventHandler<IndexMusicOnDemandCompletedEventArgs>(indexMusicCompletedCallback);
            ((MusicClient)_svc).IndexLyricsOnDemandCompleted += new EventHandler<IndexLyricsOnDemandCompletedEventArgs>(indexLyricsCompletedCallback);

                switch (indexType)
                {
                    case IndexType.Music:
                        ((MusicClient)_svc).IndexMusicOnDemandAsync(fromPath);
                        break;
                    case IndexType.Lyrics:
                        ((MusicClient)_svc).IndexLyricsOnDemandAsync(fromPath);
                        break;
                    default:
                        throw new Exception("Unnown type of index");
                }
        }

        public ProgressStatusModel[] GetMonitoring()
        {
            try
            {
                return _svc.GetLastProcessedPackages().Select(p => new ProgressStatusModel { Id = p.Id.ToString(), Package = p.Package.ToString(CultureInfo.InvariantCulture), Status = p.Status.ToString(), CreateDateTime = p.MessageCreationDateTime }).ToArray();
            }
            catch(Exception ex)
            {
                return new ProgressStatusModel[] { new ProgressStatusModel { CreateDateTime = DateTime.Now, Id = "Unknown error on service", Status = "Error" ,Package="Most likely the server has timed out."} };
            }

        }


        public void StartAcquyringLyrics(string fromPath, bool retryExisting,Action<object,AcquireLyricsOnDemandForCompletedEventArgs> acquireLyricsCallBack)
        {
            ((MusicClient)_svc).AcquireLyricsOnDemandForCompleted += new EventHandler<AcquireLyricsOnDemandForCompletedEventArgs>(acquireLyricsCallBack);
            ((MusicClient)_svc).AcquireLyricsOnDemandForAsync(fromPath, retryExisting);
        }

        public void Dispose()
        {
            _svc = null;
        }
    }
}
