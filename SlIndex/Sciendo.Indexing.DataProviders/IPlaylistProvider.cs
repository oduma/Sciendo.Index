using Sciendo.Music.Contracts.Solr;
using Sciendo.Music.DataProviders.Models.Query;

namespace Sciendo.Music.DataProviders
{
    public interface IPlaylistProvider
    {
        ResultsPackage GetFullDocuments(int page, string userName, string lastFmBaseApiUrl, string lastFmApiKey, IResultsProvider resultsProvider);

        string CreatePlaylistPackage();

        void AddToPlaylist(string[] filePaths);

        void ResetPlaylist();

    }
}
