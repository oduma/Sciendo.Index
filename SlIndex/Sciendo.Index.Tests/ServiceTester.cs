using NUnit.Framework;
using Sciendo.Music.Agent.Service;
using Sciendo.Music.Mocks.Processing;
using Sciendo.Music.Real.Procesors.MusicSourced;

namespace Sciendo.Music.Tests
{
    [TestFixture]
    public class ServiceTester
    {
        [Test]
        public void IndexAFolderOk()
        {
            IndexingFilesProcessor musicFilesProcessor= new MockIndexingFilesProcessor();
            MusicService svc = new MusicService(musicFilesProcessor,null);
            Assert.AreEqual(6,svc.IndexOnDemand(@"TestData\Music"));
        }

        [Test]
        public void IndexAFileOk()
        {
            IndexingFilesProcessor musicFilesProcessor = new MockIndexingFilesProcessor();
            MusicService svc = new MusicService(musicFilesProcessor, null);
            Assert.AreEqual(1, svc.IndexOnDemand(@"TestData\Music\MockMp3.mp3"));
        }

        [Test]
        public void GetSourceFolderOk()
        {
            IndexingFilesProcessor musicFilesProcessor = new MockIndexingFilesProcessor();
            MusicService svc = new MusicService(musicFilesProcessor, null);
            Assert.AreEqual(@"TestData\Music",svc.GetSourceFolder());
        }

    }
}
