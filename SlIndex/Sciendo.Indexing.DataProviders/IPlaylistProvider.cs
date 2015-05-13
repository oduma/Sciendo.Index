using Sciendo.Music.Contracts.Solr;
using Sciendo.Music.DataProviders.Models.Playlist;
using Sciendo.Music.DataProviders.Models.Query;

namespace Sciendo.Music.DataProviders
{
    public interface IPlaylistProvider
    {
        PlaylistPageModel GetNewPlaylistPage(int page, string userName, string lastFmBaseApiUrl, string lastFmApiKey, IResultsProvider resultsProvider);

        string CreatePlaylistPackage();

        void AddToPlaylist(string[] filePaths);

        void ResetPlaylist();

        PlaylistModel RefreshPlaylist(string lastFmUserName,  string lastFmBaseApiUrl, string lastFmApiKey, IResultsProvider resultsProvider);
    }
}
