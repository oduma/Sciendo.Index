using System;
using Sciendo.Music.DataProviders.Models.Query;

namespace Sciendo.Music.DataProviders
{
    public interface IResultsProvider
    {
        ResultsPackage GetResultsPackage(string query, int numRows, int startRow, ISolrQueryStrategy solrQueryStrategy,
            Func<string, Func<string>,SolrResponse> retrieverMethod);


        ResultsPackage GetFilteredResultsPackage(string criteria, int numRows, int startRow, string facetFieldName,
            string facetFieldValue, ISolrQueryStrategy solrQueryStrategy,
            Func<string, Func<string>,SolrResponse> retrieverMethod);
    }
}