using Sciendo.Music.Solr.Configuration;
using Sciendo.Music.Solr.Query.FromSolr;
using Sciendo.Music.Solr.Query.ToConsummer;
using Sciendo.Music.Solr.Strategies;
using System;
using System.Configuration;


namespace Sciendo.Music.Solr.Query
{
    public class SolrResultsProvider:ResultsProviderBase
    {

        public override ResultsPackage GetResultsPackageWithPreciseStrategy(string query, int numRows, int startRow, RequestType requestType)
        {
            var queryString = (requestType==RequestType.Get)?new SolrPreciseQueryStrategy(query).GetQueryString():query;
            var solrResponse =
                WebRetriever.TryRetrieve<SolrResponse>(
                    ((QueryConfigurationSection)
                        ConfigurationManager.GetSection(QueryConfigurationName.QueryProviderConfigurationName))
                        .SolrConnectionString, queryString,requestType);
            if (solrResponse == null)
                return null;

            return new ResultsPackage
            {
                ResultRows = ApplyHighlights(solrResponse),
                Fields = GetFields(solrResponse),
                PageInfo = GetNewPageInfo(solrResponse, numRows, startRow)
            };

        }

        public override ResultsPackage GetResultsPackageWithVagueStrategy(string query, int numRows, int startRow, RequestType requestType)
        {
            var queryString = new SolrVagueQueryStrategy(query,numRows,startRow).GetQueryString();
            var solrResponse =
                WebRetriever.TryRetrieve<SolrResponse>(
                    ((QueryConfigurationSection)
                        ConfigurationManager.GetSection(QueryConfigurationName.QueryProviderConfigurationName))
                        .SolrConnectionString, queryString, requestType);
            if (solrResponse == null)
                return null;

            return new ResultsPackage
            {
                ResultRows = ApplyHighlights(solrResponse),
                Fields = GetFields(solrResponse),
                PageInfo = GetNewPageInfo(solrResponse, numRows, startRow)
            };
        }

        public override ResultsPackage GeFilteredtResultsPackageWithVagueStrategy(string query, int numRows, int startRow, string facetFieldName, string faceFieldValue, RequestType requestType)
        {
            var queryString = new SolrVagueQueryStrategy(query, numRows, startRow,facetFieldName,faceFieldValue).GetFilterString();
            var solrResponse =
                WebRetriever.TryRetrieve<SolrResponse>(
                    ((QueryConfigurationSection)
                        ConfigurationManager.GetSection(QueryConfigurationName.QueryProviderConfigurationName))
                        .SolrConnectionString, queryString, requestType);
            if (solrResponse == null)
                return null;

            return new ResultsPackage
            {
                ResultRows = ApplyHighlights(solrResponse),
                Fields = GetFields(solrResponse),
                PageInfo = GetNewPageInfo(solrResponse, numRows, startRow)
            };
        }
    }
}
