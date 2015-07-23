using System;
using System.Globalization;
using System.Linq;
using Sciendo.Common.Logging;
using Sciendo.Music.DataProviders.Models.Indexing;


namespace Sciendo.Music.DataProviders
{
    public sealed class DataProvider:IDataProvider
    {
        private IMusic _svc = new MusicClient();

        public string[] GetIndexingAutocomplete(string term)
        {
            
            return _svc.ListAvailablePathsForIndexing(term);
        }

        public string GetSourceFolder()
        {
            try
            {
                var formattedSourceFolder = _svc.GetSourceFolder();
                return formattedSourceFolder.Replace("\\", "/");

            }
            catch(Exception ex)
            {
                LoggingManager.LogSciendoSystemError("Possibly the agent is down or not responding.", ex);
                return string.Empty;
            }
        }

        public void StartIndexing(string fromPath, Action<object,IndexOnDemandCompletedEventArgs> indexCompletedCallback)
        {
            ((MusicClient)_svc).IndexOnDemandCompleted += new EventHandler<IndexOnDemandCompletedEventArgs>(indexCompletedCallback);
            ((MusicClient)_svc).IndexOnDemandAsync(fromPath);
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
