using Sciendo.Music.Solr.Query.FromSolr;
using Sciendo.Music.Solr.Query.ToConsummer;
using Sciendo.Music.Solr.Strategies;
using System;

namespace Sciendo.Music.Solr.Query
{
    public interface IResultsProvider
    {
        ResultsPackage GetResultsPackageWithPreciseStrategy(string query, int numRows, int startRow, RequestType requestType);

        ResultsPackage GetResultsPackageWithVagueStrategy(string query, int numRows, int startRow, RequestType requestType);

        ResultsPackage GeFilteredtResultsPackageWithVagueStrategy(string query, int numRows, int startRow, string facetFieldName, string faceFieldValue, RequestType requestType);
    }
}