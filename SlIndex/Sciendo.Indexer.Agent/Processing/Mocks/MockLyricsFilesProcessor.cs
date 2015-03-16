using Sciendo.Common.Logging;
using Sciendo.Lyrics.Common;
using Sciendo.Lyrics.Common.Mocks;
using Sciendo.Music.Agent;
using Sciendo.Music.Agent.Processing;
using Sciendo.Music.Agent.Service.Solr.Mocks;

namespace Sciendo.Indexer.Agent.Processing.Mocks
{
    public class MockLyricsFilesProcessor : LyricsFilesProcessor
    {
        public MockLyricsFilesProcessor(string lyricsRootPath, string musicRootPath)
            : base(new AgentConfigurationSource {SourceDirectory = lyricsRootPath,SearchPattern="*.lrc"}, musicRootPath)
        {
            LoggingManager.Debug("Constructing MockLyricsFilesprocessor...");
            Sender = new MockSender();
            LyricsDeserializer=new MockLyricsDeserializer();
            LoggingManager.Debug("MockLyricsFilesprocessor constructed.");
        }
    }
}
