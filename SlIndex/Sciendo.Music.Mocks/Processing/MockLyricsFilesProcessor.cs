using Sciendo.Common.Logging;
using Sciendo.Music.Mocks.Solr;
using Sciendo.Music.Real.Procesors.Configuration;
using Sciendo.Music.Real.Procesors.LyricsSourced;

namespace Sciendo.Music.Mocks.Processing
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

        public MockLyricsFilesProcessor(): base()
        {
            LoggingManager.Debug("Constructing MockLyricsFilesprocessor...");
            Sender = new MockSender();
            LyricsDeserializer = new MockLyricsDeserializer();
            LoggingManager.Debug("MockLyricsFilesprocessor constructed.");
        }
    }
}
