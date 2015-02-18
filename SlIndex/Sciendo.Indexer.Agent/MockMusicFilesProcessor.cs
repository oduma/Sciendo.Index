using System.Collections.Generic;
using System.Linq;
using Sciendo.Common.Logging;
using Sciendo.Index.Solr;

namespace Sciendo.Indexer.Agent
{
    public class MockMusicFilesProcessor:FilesProcessor
    {
        public MockMusicFilesProcessor()
        {
            LoggingManager.Debug("Constructing MockMusicFilesprocessor...");
            Sender = new MockSender();
            LoggingManager.Debug("MockMusicFilesprocessor constructed.");
        }

        protected override IEnumerable<Document> PrepareDocuments(IEnumerable<string> files)
        {
            yield return new FullDocument(files.First(), "t", new string[] { "test artist" }, "test song", "test alubm");
        }
    }
}
