using Sciendo.Indexer.Agent;
using Sciendo.Lyrics.Common;

namespace Sciendo.Index.Tests
{
    public class MockLyricsFilesProcessor:LyricsFilesProcessor
    {
        public MockLyricsFilesProcessor(string musicRootFolder):base(musicRootFolder,new MockLyricsDeserializer())
        {

        }
    }
}
