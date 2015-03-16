using NUnit.Framework;
using Sciendo.Indexer.Agent.Processing;
using Sciendo.Indexer.Agent.Processing.Mocks;
using Sciendo.Music.Agent.Processing;
using Sciendo.Music.Agent.Processing.Mocks;
using Sciendo.Music.Agent.Service;

namespace Sciendo.Index.Tests
{
    [TestFixture]
    public class ServiceTester
    {
        [Test]
        public void IndexAMusicFolderOk()
        {
            MusicFilesProcessor musicFilesProcessor= new MockMusicFilesProcessor();
            LyricsFilesProcessor lyricsFilesProcessor = new MockLyricsFilesProcessor(@"TestData\Lyrics",
                @"TestData\Music");
            MusicService svc = new MusicService(musicFilesProcessor,lyricsFilesProcessor,null,2);
            Assert.AreEqual(2,svc.IndexMusicOnDemand(@"TestData\Music"));
        }

        [Test]
        public void IndexALyricsFolderOk()
        {
            MusicFilesProcessor musicFilesProcessor = new MockMusicFilesProcessor();
            LyricsFilesProcessor lyricsFilesProcessor = new MockLyricsFilesProcessor(@"TestData\Lyrics",
                @"TestData\Music");
            MusicService svc = new MusicService(musicFilesProcessor, lyricsFilesProcessor, null,2);
            Assert.AreEqual(2, svc.IndexLyricsOnDemand(@"TestData\Lyrics"));
        }

        [Test]
        public void IndexAMusicFileOk()
        {
            MusicFilesProcessor musicFilesProcessor = new MockMusicFilesProcessor();
            LyricsFilesProcessor lyricsFilesProcessor = new MockLyricsFilesProcessor(@"TestData\Lyrics",
                @"TestData\Music");
            MusicService svc = new MusicService(musicFilesProcessor, lyricsFilesProcessor, null,2);
            Assert.AreEqual(1, svc.IndexMusicOnDemand(@"TestData\Music\MockMp3.mp3"));
        }

        [Test]
        public void IndexALyricsFileOk()
        {
            MusicFilesProcessor musicFilesProcessor = new MockMusicFilesProcessor();
            LyricsFilesProcessor lyricsFilesProcessor = new MockLyricsFilesProcessor(@"TestData\Lyrics",
                @"TestData\Music");
            MusicService svc = new MusicService(musicFilesProcessor, lyricsFilesProcessor, null,2);
            Assert.AreEqual(1, svc.IndexLyricsOnDemand(@"TestData\Lyrics\MockMp3.lrc"));
        }

    }
}
