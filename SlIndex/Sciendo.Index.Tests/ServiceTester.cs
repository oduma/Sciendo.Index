using NUnit.Framework;
using Sciendo.Indexer.Agent.Processing;
using Sciendo.Indexer.Agent.Processing.Mocks;
using Sciendo.Indexer.Agent.Service;

namespace Sciendo.Index.Tests
{
    [TestFixture]
    public class ServiceTester
    {
        [Test]
        public void IndexAMusicFolderOk()
        {
            FilesProcessor musicFilesProcessor= new MockMusicFilesProcessor();
            LyricsFilesProcessor lyricsFilesProcessor = new MockLyricsFilesProcessor(@"TestData\Lyrics",
                @"TestData\Music");
            IndexerAgentService svc = new IndexerAgentService(musicFilesProcessor,lyricsFilesProcessor,2);
            Assert.AreEqual(2,svc.IndexMusicOnDemand(@"TestData\Music"));
        }

        [Test]
        public void IndexALyricsFolderOk()
        {
            FilesProcessor musicFilesProcessor = new MockMusicFilesProcessor();
            LyricsFilesProcessor lyricsFilesProcessor = new MockLyricsFilesProcessor(@"TestData\Lyrics",
                @"TestData\Music");
            IndexerAgentService svc = new IndexerAgentService(musicFilesProcessor, lyricsFilesProcessor, 2);
            Assert.AreEqual(2, svc.IndexLyricsOnDemand(@"TestData\Lyrics"));
        }

        [Test]
        public void IndexAMusicFileOk()
        {
            FilesProcessor musicFilesProcessor = new MockMusicFilesProcessor();
            LyricsFilesProcessor lyricsFilesProcessor = new MockLyricsFilesProcessor(@"TestData\Lyrics",
                @"TestData\Music");
            IndexerAgentService svc = new IndexerAgentService(musicFilesProcessor, lyricsFilesProcessor, 2);
            Assert.AreEqual(1, svc.IndexMusicOnDemand(@"TestData\Music\MockMp3.mp3"));
        }

        [Test]
        public void IndexALyricsFileOk()
        {
            FilesProcessor musicFilesProcessor = new MockMusicFilesProcessor();
            LyricsFilesProcessor lyricsFilesProcessor = new MockLyricsFilesProcessor(@"TestData\Lyrics",
                @"TestData\Music");
            IndexerAgentService svc = new IndexerAgentService(musicFilesProcessor, lyricsFilesProcessor, 2);
            Assert.AreEqual(1, svc.IndexLyricsOnDemand(@"TestData\Lyrics\MockMp3.lrc"));
        }

    }
}
