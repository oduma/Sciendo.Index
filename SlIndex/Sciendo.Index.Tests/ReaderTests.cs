using Newtonsoft.Json;
using NUnit.Framework;
using Sciendo.Index.Solr;
using Sciendo.Indexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReaderLyricsTestsNoMusicPathGiven()
        {
            MockLyricsFilesProcessor processor = new MockLyricsFilesProcessor(@"");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage="Invalid path")]
        public void ReaderLyricsTestsInvaldiMusicPathGiven()
        {
            MockLyricsFilesProcessor processor = new MockLyricsFilesProcessor(@"c:\something\something");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid path")]
        public void ReaderLyricsTestsInvaldiMusicPathFileGiven()
        {
            MockLyricsFilesProcessor processor = new MockLyricsFilesProcessor(@"TestData\Music\MockMp31.mp3");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid path")]
        public void ReaderLyricsTestsInvalidLyricsPathGiven()
        {
            MockLyricsFilesProcessor processor = new MockLyricsFilesProcessor(@"TestData\Music");
            Reader reader = new Reader(null);
            reader.ProcessFiles = processor.ProcessFilesBatch;
            reader.ParsePath(@"C:\something\something", "*.lrc");
        }
        [Test]
        public void ReaderLyricsTestsFolderOk()
        {
            MockLyricsFilesProcessor processor = new MockLyricsFilesProcessor(@"TestData\Music");
            Reader reader = new Reader(null);
            reader.ProcessFiles = processor.ProcessFilesBatch;
            reader.ParsePath(@"TestData\Lyrics", "*.lrc");
            Assert.AreEqual(2,processor.Counter);
        }
        [Test]
        public void ReaderLyricsTestsFileOk()
        {
            MockLyricsFilesProcessor processor = new MockLyricsFilesProcessor(@"TestData\Music\Sub");
            Reader reader = new Reader(null);
            reader.ProcessFiles = processor.ProcessFilesBatch;
            reader.ParsePath(@"TestData\Lyrics\Sub\MockOgg.lrc", "*.lrc");
            Assert.AreEqual(1, processor.Counter);
        }
    }
}
