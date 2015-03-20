using System;
using Sciendo.Indexing.DataProviders.Models;
using Sciendo.Music.Contracts.MusicService;

namespace Sciendo.Indexing.DataProviders
{
    public interface IDataProvider:IDisposable
    {
        string[] GetMuiscAutocomplete(string term);
        string[] GetLyricsAutocomplete(string term);
        SourceFolders GetSourceFolders();
        IndexingResult StartIndexing(string fromPath, IndexType indexType);
        ProgressStatusModel[] GetMonitoring();
    }
}
