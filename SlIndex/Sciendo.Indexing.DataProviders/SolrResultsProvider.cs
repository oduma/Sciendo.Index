using System;
using System.Configuration;
using Sciendo.Music.DataProviders.Common;
using Sciendo.Music.DataProviders.Configuration;
using Sciendo.Music.DataProviders.Models.Query;

namespace Sciendo.Music.DataProviders
{
    public class SolrResultsProvider:ResultsProviderBase
    {
        public override ResultsPackage GetResultsPackage(string query, int numRows, int startRow, ISolrQueryStrategy solrQueryStrategy,
                    Func<string, Func<string>,SolrResponse> retrieverMethod)
        {
            var solrResponse =
                retrieverMethod(
                    ((QueryConfigurationSection)
                        ConfigurationManager.GetSection(ConfigurationSectionNames.QueryProviderConfigurationName))
                        .SolrConnectionString, solrQueryStrategy.GetQueryString);
            if (solrResponse == null)
                return null;

            return new ResultsPackage
            {
                ResultRows = ApplyHighlights(solrResponse),
                Fields = GetFields(solrResponse),
                PageInfo=GetNewPageInfo(solrResponse,numRows,startRow)
            };

        }

        public override ResultsPackage GetFilteredResultsPackage(string criteria, int numRows, int startRow, string facetFieldName,
            string facetFieldValue, ISolrQueryStrategy solrQueryStrategy, Func<string, Func<string>,SolrResponse> retrieverMethod)
        {
            var solrResponse =
                retrieverMethod(
                    ((QueryConfigurationSection)
                        ConfigurationManager.GetSection(ConfigurationSectionNames.QueryProviderConfigurationName))
                        .SolrConnectionString, solrQueryStrategy.GetFilterString);
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
