using System;
using Sciendo.Music.DataProviders.Models.Indexing;
using Sciendo.Music.DataProviders.MusicClient;

namespace Sciendo.Music.DataProviders
{
    public interface IDataProvider:IDisposable
    {
        string[] GetMuiscAutocomplete(string term);
        string[] GetLyricsAutocomplete(string term);
        SourceFolders GetSourceFolders();
        IndexingResult StartIndexing(string fromPath, IndexType indexType);
        ProgressStatusModel[] GetMonitoring();
        IndexingResult StartAcquyringLyrics(string fromPath, bool retryExisting);
    }
}
