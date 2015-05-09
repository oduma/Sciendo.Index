using Sciendo.Music.DataProviders.Models.Query;

namespace Sciendo.Music.DataProviders
{
    public interface IResultsProvider
    {
        ResultsPackage GetResultsPackage(string query, int numRow, int startRow,ISolrQueryStrategy solrQueryStrategy);


        ResultsPackage GetFilteredResultsPackage(string criteria, int numRow, int startRow, string facetFieldName, string facetFieldValue, ISolrQueryStrategy solrQueryStrategy);
    }
}