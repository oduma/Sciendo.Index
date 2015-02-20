using Sciendo.Common.Logging;
using Sciendo.Indexer.Agent.Service.Solr.Mocks;
using Sciendo.Lyrics.Common;

namespace Sciendo.Indexer.Agent.Processing.Mocks
{
    public class MockLyricsFilesProcessor : LyricsFilesProcessor
    {
        public MockLyricsFilesProcessor(string lyricsRootPath, string musicRootPath)
            : base(new IndexerConfigurationSource {SourceDirectory = lyricsRootPath,SearchPattern="*.lrc"}, musicRootPath)
        {
            LoggingManager.Debug("Constructing MockLyricsFilesprocessor...");
            Sender = new MockSender();
            LyricsDeserializer=new MockLyricsDeserializer();
            LoggingManager.Debug("MockLyricsFilesprocessor constructed.");
        }
    }
}
