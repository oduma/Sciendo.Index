using Rhino.Mocks;
using Sciendo.Music.Solr;
using Sciendo.Music.Solr.Query;
using Sciendo.Music.Solr.Query.Common;
using Sciendo.Music.Solr.Query.FromSolr;
using Sciendo.Music.Solr.Query.ToConsummer;
using Sciendo.Music.Solr.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Tests
{
    public enum MissingType
    {
        None,
        Album,
        Artist,
        Title,
        Lyrics,
        All
    }
    public static class SolrResultsProviderMocks
    {
        public static IResultsProvider GetExceptionMock(string query)
        {
            var solrQuery = string.Format("file_path_id:\"{0}\"", query);
            var mockResultsProvider = MockRepository.GenerateStub<IResultsProvider>();
            mockResultsProvider.Stub(m => m.GetResultsPackageWithPreciseStrategy(solrQuery,1,0,RequestType.Post)).Throw(new Exception());
            return mockResultsProvider;

        }

        internal static IResultsProvider GetNullResultsMock(string query)
        {
            var solrQuery = string.Format("file_path_id:\"{0}\"", query);
            var mockResultsProvider = MockRepository.GenerateStub<IResultsProvider>();
            mockResultsProvider.Stub(m => m.GetResultsPackageWithPreciseStrategy(solrQuery, 1, 0, RequestType.Post)).Return(null);
            return mockResultsProvider;
        }

        internal static IResultsProvider GetNullResultRowsMock(string query)
        {
            var solrQuery = string.Format("file_path_id:\"{0}\"", query);
            var mockResultsProvider = MockRepository.GenerateStub<IResultsProvider>();
            mockResultsProvider.Stub(m => m.GetResultsPackageWithPreciseStrategy(solrQuery, 1, 0, RequestType.Post)).Return(new ResultsPackage());
            return mockResultsProvider;
        }

        internal static IResultsProvider GetNoResultRowsMock(string query)
        {
            var solrQuery = string.Format("file_path_id:\"{0}\"", query);
            var mockResultsProvider = MockRepository.GenerateStub<IResultsProvider>();
            mockResultsProvider.Stub(m => m.GetResultsPackageWithPreciseStrategy(solrQuery, 1, 0, RequestType.Post)).Return(new ResultsPackage { ResultRows=new Doc[]{}});
            return mockResultsProvider;
        }

        internal static IResultsProvider GetOneEmptyResultRowsMock(string query)
        {
            var solrQuery = string.Format("file_path_id:\"{0}\"", query);
            var mockResultsProvider = MockRepository.GenerateStub<IResultsProvider>();
            mockResultsProvider.Stub(m => m.GetResultsPackageWithPreciseStrategy(solrQuery, 1, 0, RequestType.Post)).Return(new ResultsPackage { ResultRows = new Doc[] { null} });
            return mockResultsProvider;
        }

        internal static IResultsProvider GetIndexedResultMock(string query, MissingType missingType)
        {
            Doc returnedDoc = GetDoc(missingType);
            var solrQuery = string.Format("file_path_id:\"{0}\"", query);
            var mockResultsProvider = MockRepository.GenerateStub<IResultsProvider>();
            mockResultsProvider.Stub(m => m.GetResultsPackageWithPreciseStrategy(solrQuery, 1, 0, RequestType.Post)).Return(new ResultsPackage { ResultRows = new Doc[] { returnedDoc } });
            return mockResultsProvider;

        }

        private static Doc GetDoc(MissingType missingType)
        {
            return new Doc
            {
                Album = (missingType == MissingType.Album || missingType==MissingType.All) ? "" : "my album",
                Artist = (missingType == MissingType.Artist || missingType == MissingType.All) ? null : new string[] { "my artist" },
                Title = (missingType == MissingType.Title || missingType == MissingType.All) ? "" : "my song",
                Lyrics = (missingType == MissingType.Lyrics || missingType == MissingType.All) ? "" : "my lyrics"
            };
        }
    }
}
