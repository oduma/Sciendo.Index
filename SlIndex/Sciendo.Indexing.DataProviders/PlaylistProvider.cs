using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Music.Contracts.Solr;
using Sciendo.Music.DataProviders.Common;
using Sciendo.Music.DataProviders.Models.Playlist.LastFm;
using Sciendo.Music.DataProviders.Models.Query;
using Sciendo.Music.DataProviders.Models.Playlist;
using System.IO.Compression;

namespace Sciendo.Music.DataProviders
{
    public class PlaylistProvider:IPlaylistProvider
    {
        private string _lastFmBaseApiUrl;
        private string _lastFmApiKey;
        private string _lastFmUser;
        private int _page;

        public PlaylistPageModel GetNewPlaylistPage(int page, string userName,string lastFmBaseApiUrl, string lastFmApiKey,IResultsProvider resultsProvider)
        {
            _lastFmBaseApiUrl = lastFmBaseApiUrl;
            _lastFmApiKey = lastFmApiKey;
            _page = page;
            _lastFmUser = userName;

            var results = GetPlaylistPage(resultsProvider);
            return results;
            }

        public byte[] CreatePlaylistPackage(PlaylistModel playlistModel)
        {
            //create the plu file
            string m3UString = CreateM3UString(playlistModel);
            //create batch copy file
            string batchString = CreateBatchString(playlistModel);
            //zipp them toigether and send the byte array
            return ZipResponseToMemory(m3UString, batchString);
        }

        private byte[] ZipResponseToMemory(string m3UString, string batchString)
        {
            using (MemoryStream zipStream = new MemoryStream())
            {
                
                using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create,true))
                {
                    ZipArchiveEntry playlistEntry = archive.CreateEntry("playlist.m3u");
                    using (StreamWriter writer = new StreamWriter(playlistEntry.Open()))
                    {
                        writer.Write(m3UString);
                    }
                    ZipArchiveEntry batchFile = archive.CreateEntry("filecopy.bat");
                    using (StreamWriter writer = new StreamWriter(batchFile.Open()))
                    {
                        writer.Write(batchString);
                    }
                }
                long size = zipStream.Length;
                var resultBuffer = new byte[size];
                zipStream.Position = 0;
                zipStream.Read(resultBuffer, 0, (int)size);
                return resultBuffer;
            }
        }

        private string CreateBatchString(PlaylistModel playlistModel)
        {
            return string.Join("\r\n", playlistModel.Pages.SelectMany(p => p.ResultRows).Select((i) =>string.Format("copy \"{0}\" *",i.FilePath.Replace(@"file:///",""))));
        }

        private string CreateM3UString(PlaylistModel playlistModel)
        {
            return string.Join("\r\n", playlistModel.Pages.SelectMany(p => p.ResultRows).Select((i) =>
            {
                var iarray = i.FilePath.Split(new[] {'\\'});
                return string.Format("\"{0}\"",iarray[iarray.Length - 1]);
            }));
        }

        private PlaylistPageModel GetPlaylistPage(IResultsProvider resultsProvider)
        {
            LastFmResponse lastFmResponse = GetLastFmResponse();

            var solrQuery = TranslateLastFmToSolr(lastFmResponse);

            var results =
                resultsProvider.GetResultsPackage(solrQuery
                    , lastFmResponse.LovedTracks.Info.PerPage, 0, new SolrPreciseQueryStrategy(solrQuery), 
                    WebRetriever.TryPost<SolrResponse>);

            var playlistPage = new PlaylistPageModel();
            playlistPage.PageInfo=
                new PageInfo
                {
                    PageStartRow =
                        (lastFmResponse.LovedTracks.Info.Page - 1) * lastFmResponse.LovedTracks.Info.PerPage,
                    RowsPerPage = lastFmResponse.LovedTracks.Info.PerPage,
                    TotalRows = lastFmResponse.LovedTracks.Info.Total
                };
            playlistPage.ResultRows = results.ResultRows.Select(r => new PlaylistItem { FilePathId = r.FilePathId, FilePath = r.FilePath, Album = r.Album, Artist = r.Artist, Lyrics = r.Lyrics, Title = r.Title }).ToArray();
            return playlistPage;
        }

        protected virtual string TranslateLastFmToSolr(LastFmResponse lastFmResponse)
        {
            return string.Join(" OR ",
                lastFmResponse.LovedTracks.Tracks.Select(
                    t =>
                        string.Format("(artist_f:\"{0}\" AND title_f:\"{1}\")", t.LovedArtist.Name.ToLower(),
                            t.Name.ToLower())));
        }

        protected virtual LastFmResponse GetLastFmResponse()
        {
           return WebRetriever.TryGet<LastFmResponse>(_lastFmBaseApiUrl,
                LastFmQueryStringProvider);
        }

        private string LastFmQueryStringProvider()
        {
            return "method=user.getlovedtracks&page=" + _page + "&user=" + _lastFmUser + "&api_key=" + _lastFmApiKey +
                   "&format=json";
        }

        public PlaylistModel RefreshPlaylist(string lastFmUserName, string lastFmBaseApiUrl, string lastFmApiKey, IResultsProvider resultsProvider)
        {
            _lastFmBaseApiUrl = lastFmBaseApiUrl;
            _lastFmApiKey = lastFmApiKey;
            _page = 1;
            _lastFmUser = lastFmUserName;
            var playlist = new PlaylistModel();
            playlist.Pages.Add(GetPlaylistPage(resultsProvider));
            return playlist;
        }
    }
}
