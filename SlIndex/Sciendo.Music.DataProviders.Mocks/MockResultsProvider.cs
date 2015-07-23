using System;
using System.IO;
using Newtonsoft.Json;
using Sciendo.Music.DataProviders.Models.Query;
using Sciendo.Music.DataProviders.SolrContracts;

namespace Sciendo.Music.DataProviders.Mocks
{
    public class MockResultsProvider : ResultsProviderBase
    {
        public override ResultsPackage GetResultsPackage(string query, int numRows, int startRow, ISolrQueryStrategy solrQueryStrategy,
            Func<string, Func<string>, SolrResponse> retrieverMethod)
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"App_data");
            var mockFilePath =
                    Path.Combine(dir,"examplequeryresult.json");
            return Deserialize(mockFilePath, numRows, startRow);
        }

        private ResultsPackage Deserialize(string mockFilePath,int numRows, int startRow)
        {
            using (var fs = File.OpenText(mockFilePath))
            {
                var txt = fs.ReadToEnd();
                var solrResponse = JsonConvert.DeserializeObject<SolrResponse>(txt, new DictionariesConverter());

                return new ResultsPackage
                {
                    ResultRows = ApplyHighlights(solrResponse),
                    Fields = GetFields(solrResponse),
                    PageInfo=GetNewPageInfo(solrResponse,numRows,startRow)
                };
            }
        }


        public override ResultsPackage GetFilteredResultsPackage(string criteria, int numRows, int startRow, string facetFieldName,
            string facetFieldValue, ISolrQueryStrategy solrQueryStrategy, Func<string, Func<string>, SolrResponse> retrieverMethod)
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_data");
            var mockFilePath =
                    Path.Combine(dir, "filteredjsonexample.json");
            return Deserialize(mockFilePath,numRows,startRow);
        }


    }
}