using Sciendo.Common.Logging;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;

namespace Sciendo.Indexer.Agent
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
