using System;
using Sciendo.Music.Contracts.MusicService;
using Sciendo.Music.DataProviders.Models;

namespace Sciendo.Music.DataProviders
{
    public interface IDataProvider:IDisposable
    {
        string[] GetMuiscAutocomplete(string term);
        string[] GetLyricsAutocomplete(string term);
        SourceFolders GetSourceFolders();
        IndexingResult StartIndexing(string fromPath, IndexType indexType);
        ProgressStatusModel[] GetMonitoring();
        int AquireLyrics(string fromPath, bool retryExisting);
    }
}
