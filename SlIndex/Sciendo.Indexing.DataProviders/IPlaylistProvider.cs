using Sciendo.Music.DataProviders.Models.Playlist;
using Sciendo.Music.Solr.Query;

namespace Sciendo.Music.DataProviders
{
    public interface IPlaylistProvider
    {
        PlaylistPageModel GetNewPlaylistPage(int page, string userName, string lastFmBaseApiUrl, string lastFmApiKey, IResultsProvider resultsProvider);

        byte[] CreatePlaylistPackage(PlaylistModel playlistModel);

        PlaylistModel RefreshPlaylist(string lastFmUserName,  string lastFmBaseApiUrl, string lastFmApiKey, IResultsProvider resultsProvider);
    }
}
