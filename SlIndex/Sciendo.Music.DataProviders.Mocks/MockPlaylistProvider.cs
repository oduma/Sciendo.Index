using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sciendo.Music.DataProviders.Models.Playlist.LastFm;
using Sciendo.Music.DataProviders.Models.Query;

namespace Sciendo.Music.DataProviders.Mocks
{
    public class MockPlaylistProvider:PlaylistProvider
    {
        private LastFmResponse Deserialize(string mockFilePath)
        {
            using (var fs = File.OpenText(mockFilePath))
            {
                var txt = fs.ReadToEnd();
                var solrResponse = JsonConvert.DeserializeObject<LastFmResponse>(txt);
                return solrResponse;

                //return new ResultsPackage
                //{
                //    ResultRows = ApplyHighlights(solrResponse),
                //    Fields = GetFields(solrResponse),
                //    PageInfo = GetNewPageInfo(solrResponse, numRows, startRow)
                //};
            }
        }

        protected override string TranslateLastFmToSolr(LastFmResponse lastFmResponse)
        {
            return 
                "(artist_f:\"accept\" AND title_f:\"fast as a shark\") OR (artist_f:\"accept\" AND title_f:\"fast as a snail\")";

        }

        protected override LastFmResponse GetLastFmResponse()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_data");
            var mockFilePath =
                    Path.Combine(dir, "examplefromlastfm.json");
            return Deserialize(mockFilePath);
        }
    }
}
