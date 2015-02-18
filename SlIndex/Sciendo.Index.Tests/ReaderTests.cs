using NUnit.Framework;
using System;
using Sciendo.Indexer.Agent;

namespace Sciendo.Index.Tests
{
    [TestFixture]
    public class ReaderTests
    {

        [Test]
        [ExpectedException(typeof(ArgumentException),ExpectedMessage="Invalid path")]
        public void ReaderTestsInvalidPath()
        {
            MockMusicFilesProcessor _fileProcessor = new MockMusicFilesProcessor();
            Reader reader = new Reader(null);
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(@"c:\users\something\something", "*.mp3|*.ogg");
        }

        [Test]
        public void ReaderMusicTestsFolderOk()
        {
            MockMusicFilesProcessor _fileProcessor = new MockMusicFilesProcessor();
            Reader reader = new Reader(null);
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(@"TestData\Music", "*.mp3|*.ogg");
            Assert.AreEqual(2,_fileProcessor.Counter);
        }

        [Test]
        public void ReaderMusicTestsFileOk()
        {
            MockMusicFilesProcessor _fileProcessor = new MockMusicFilesProcessor();
            Reader reader = new Reader(null);
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(@"TestData\Music\MockMp3.mp3", "*.mp3|*.ogg");
            Assert.AreEqual(1, _fileProcessor.Counter);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid path")]
        public void ReaderLyricsTestsInvalidLyricsPathGiven()
        {
            MockLyricsFilesProcessor processor = new MockLyricsFilesProcessor(@"TestData\Lyrics", @"TestData\Music");
            Reader reader = new Reader(null);
            reader.ProcessFiles = processor.ProcessFilesBatch;
            reader.ParsePath(@"C:\something\something", "*.lrc");
        }
        [Test]
        public void ReaderLyricsTestsFolderOk()
        {
            MockLyricsFilesProcessor processor = new MockLyricsFilesProcessor(@"TestData\Lyrics", @"TestData\Music");
            Reader reader = new Reader(null);
            reader.ProcessFiles = processor.ProcessFilesBatch;
            reader.ParsePath(@"TestData\Lyrics", "*.lrc");
            Assert.AreEqual(2,processor.Counter);
        }
        [Test]
        public void ReaderLyricsTestsFileOk()
        {
            MockLyricsFilesProcessor processor = new MockLyricsFilesProcessor(@"TestData\Lyrics", @"TestData\Music");
            Reader reader = new Reader(null);
            reader.ProcessFiles = processor.ProcessFilesBatch;
            reader.ParsePath(@"TestData\Lyrics\Sub\MockOgg.lrc", "*.lrc");
            Assert.AreEqual(1, processor.Counter);
        }
    }
}
