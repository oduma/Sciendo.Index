using System.Configuration;
using Sciendo.Music.DataProviders.Configuration;
using Sciendo.Music.DataProviders.Models.Query;

namespace Sciendo.Music.DataProviders
{
    public class SolrResultsProvider:ResultsProviderBase
    {
        public override ResultsPackage GetResultsPackage(string query,int numRows, int startRow)
        {
            var solrResponse = SolRetriever.TryQuery(((QueryConfigurationSection)ConfigurationManager.GetSection("query")).SolrConnectionString, (new SolrQueryStrategy(query,numRows,startRow)).GetQueryString);
            if (solrResponse == null)
                return null;

            return new ResultsPackage
            {
                ResultRows = ApplyHighlights(solrResponse),
                Fields = GetFields(solrResponse),
                PageInfo=GetNewPageInfo(solrResponse,numRows,startRow)
            };

        }

        public override ResultsPackage GetFilteredResultsPackage(string criteria, int numRows, int startRow, string facetFieldName, string facetFieldValue)
        {
            var solrResponse = SolRetriever.TryQuery(((QueryConfigurationSection)ConfigurationManager.GetSection("query")).SolrConnectionString, (new SolrQueryStrategy(criteria,numRows,startRow, facetFieldName, facetFieldValue)).GetFilterString);
            if (solrResponse == null)
                return null;

            return new ResultsPackage
            {
                ResultRows = ApplyHighlights(solrResponse),
                Fields = GetFields(solrResponse),
                PageInfo=GetNewPageInfo(solrResponse,numRows,startRow)
            };
        }
    }
}
