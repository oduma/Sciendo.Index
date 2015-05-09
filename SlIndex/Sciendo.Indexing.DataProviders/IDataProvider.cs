using System;
using Sciendo.Music.DataProviders.Models.Indexing;
using Sciendo.Music.Contracts.MusicService;


namespace Sciendo.Music.DataProviders
{
    public interface IDataProvider:IDisposable
    {
        string[] GetMuiscAutocomplete(string term);
        string[] GetLyricsAutocomplete(string term);
        SourceFolders GetSourceFolders();
        void StartIndexing(string fromPath, IndexType indexType,Action<object,IndexMusicOnDemandCompletedEventArgs> indexMusicCompletedCallback,Action<object,IndexLyricsOnDemandCompletedEventArgs> indexLyricsCompletedCallback);
        ProgressStatusModel[] GetMonitoring();
        void StartAcquyringLyrics(string fromPath, bool retryExisting,Action<object,AcquireLyricsOnDemandForCompletedEventArgs> acquireLyricsCallback);
    }
}
