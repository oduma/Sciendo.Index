using System;
using Sciendo.Music.DataProviders.Models.Indexing;
using Sciendo.Music.Contracts.MusicService;


namespace Sciendo.Music.DataProviders
{
    public interface IDataProvider:IDisposable
    {
        string[] GetIndexingAutocomplete(string term);
        string GetSourceFolder();
        void StartIndexing(string fromPath, Action<object,IndexOnDemandCompletedEventArgs> indexMusicCompletedCallback);
        void StartAcquyringLyrics(string fromPath, bool retryExisting,Action<object,AcquireLyricsOnDemandForCompletedEventArgs> acquireLyricsCallback);
    }
}
