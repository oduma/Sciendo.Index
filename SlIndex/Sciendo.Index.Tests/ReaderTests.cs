using System;
using System.IO;
using NUnit.Framework;
using Sciendo.Music.Agent.Processing;
using Sciendo.Music.Mocks.Processing;

namespace Sciendo.Music.Tests
{
    [TestFixture]
    public class ReaderTests
    {
        private const string _newFileDuringTests = @"TestData\Music\Sub\NewMockMp3.mp3";
        private const string _newLrcFileDuringTests = @"TestData\Lyrics\Sub\NewMockMp3.lrc";
        private const string _newFileDuringTestsNeedsProcessing = @"TestData\Music\Sub\PostProcessing.mp3";
        private const string _newLrcFileDuringTestsNeedsProcessing = @"TestData\Lyrics\Sub\PostProcessing.lrc";

        [SetUp]
        public void Setup()
        {
            try
            {
                if (File.Exists(_newFileDuringTests))
                    File.Delete(_newFileDuringTests);
                if (File.Exists(_newLrcFileDuringTests))
                    File.Delete(_newLrcFileDuringTests);
                if (File.Exists(_newFileDuringTestsNeedsProcessing))
                    File.Delete(_newFileDuringTestsNeedsProcessing);
                if (File.Exists(_newLrcFileDuringTestsNeedsProcessing))
                    File.Delete(_newLrcFileDuringTestsNeedsProcessing);
            }
            catch
            {
            }
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (File.Exists(_newFileDuringTests))
                    File.Delete(_newFileDuringTests);
                if (File.Exists(_newLrcFileDuringTests))
                    File.Delete(_newLrcFileDuringTests);
                if (File.Exists(_newFileDuringTestsNeedsProcessing))
                    File.Delete(_newFileDuringTestsNeedsProcessing);
                if (File.Exists(_newLrcFileDuringTestsNeedsProcessing))
                    File.Delete(_newLrcFileDuringTestsNeedsProcessing);
            }
            catch
            {
            }
        }

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
        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid path")]
        public void ReaderTestsInvalidPath1()
        {
            MockMusicToLyricsFilesProcessor _fileProcessor = new MockMusicToLyricsFilesProcessor();
            Reader reader = new Reader(null);
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(@"c:\users\something\something", "*.mp3|*.ogg");
        }

        [Test]
        public void ReaderMusicToLyricsTestsFolderOkNothingNew()
        {
            MockMusicToLyricsFilesProcessor _fileProcessor = new MockMusicToLyricsFilesProcessor();
            Reader reader = new Reader(null);
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(@"TestData\Music", "*.mp3|*.ogg");
            Assert.AreEqual(0, _fileProcessor.Counter);
        }

        [Test]
        public void ReaderMusicToLyricsTestsFolderOkSomethingNew()
        {
            File.Create(_newFileDuringTests);
            MockMusicToLyricsFilesProcessor _fileProcessor = new MockMusicToLyricsFilesProcessor();
            Reader reader = new Reader(null);
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(@"TestData\Music\Sub", "*.mp3|*.ogg");
            Assert.AreEqual(1, _fileProcessor.Counter);
        }

        [Test]
        public void ReaderMusicToLyricsTestsFileOk()
        {
            MockMusicToLyricsFilesProcessor _fileProcessor = new MockMusicToLyricsFilesProcessor();
            Reader reader = new Reader(null);
            _fileProcessor.RetryExisting = true;
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(@"TestData\Music\MockMp3.mp3", "*.mp3|*.ogg");
            Assert.AreEqual(1, _fileProcessor.Counter);
        }

        [Test]
        public void ReaderMusicToLyricsTestsFileNoLyricsFound()
        {
            MockMusicToLyricsFilesProcessor _fileProcessor = new MockMusicToLyricsFilesProcessor();
            Reader reader = new Reader(null);
            _fileProcessor.RetryExisting = true;
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(@"TestData\Music\Sub\MockOgg.ogg", "*.mp3|*.ogg");
            Assert.AreEqual(0, _fileProcessor.Counter);
        }
        [Test]
        public void ReaderMusicToLyricsTestsFileNeedsPostProcessing()
        {
            File.Create(_newFileDuringTestsNeedsProcessing);
            MockMusicToLyricsFilesProcessor _fileProcessor = new MockMusicToLyricsFilesProcessor();
            Reader reader = new Reader(null);
            _fileProcessor.RetryExisting = true;
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(_newFileDuringTestsNeedsProcessing, "*.mp3|*.ogg");
            Assert.AreEqual(1, _fileProcessor.Counter);
        }
    }
}
