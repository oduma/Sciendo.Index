using Sciendo.Music.DataProviders.Models.Query;

namespace Sciendo.Music.DataProviders
{
    public interface IResultsProvider
    {
        ResultsPackage GetResultsPackage(string query, int numRow, int startRow);


        ResultsPackage GetFilteredResultsPackage(string criteria, int numRow, int startRow, string facetFieldName, string facetFieldValue);
    }
}