using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Sciendo.Common.Logging;
using Sciendo.Indexer.Agent.Service.Solr;
using Sciendo.Indexer.Agent.Service.Solr.Mocks;

namespace Sciendo.Indexer.Agent.Processing.Mocks
{
    public class MockMusicFilesProcessor:FilesProcessor
    {
        public MockMusicFilesProcessor()
        {
            LoggingManager.Debug("Constructing MockMusicFilesprocessor...");
            Sender = new MockSender();
            CurrentConfiguration = ((IndexerConfigurationSection) ConfigurationManager.GetSection("indexer")).Music;
            LoggingManager.Debug("MockMusicFilesprocessor constructed.");
        }

        protected override IEnumerable<Document> PrepareDocuments(IEnumerable<string> files)
        {
            LoggingManager.Debug("MockMusicFilesprocessor preparing documents...");
            yield return new FullDocument(files.First(), "t", new string[] { "test artist" }, "test song", "test alubm");
            LoggingManager.Debug("MockMusicFilesprocessor documents prrepared.");
        }
    }
}
