using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using Sciendo.Music.Agent.Processing;
using Sciendo.Music.Contracts.Monitoring;
using Sciendo.Music.Real.Procesors.MusicSourced;
using Sciendo.Music.Real.Feedback;
using System.Collections.Generic;
using System.Linq;

namespace Sciendo.Music.Tests
{
    [TestFixture]
    public class ReaderTests
    {
        private const string _newFileDuringTests = @"TestData\Music\Sub\NewMockMp3.mp3";
        private const string _newLrcFileDuringTests = @"TestData\Lyrics\Sub\NewMockMp3.lrc";
        private const string _newFileDuringTestsNeedsProcessing = @"TestData\Music\Sub\PostProcessing.mp3";
        private const string _newLrcFileDuringTestsNeedsProcessing = @"TestData\Lyrics\Sub\PostProcessing.lrc";
        private const string _musicFileWithoutALyricsFile = @"TestData\Music\Sub\NewMockMp3Del.mp3";

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
                if(File.Exists(_musicFileWithoutALyricsFile))
                    File.Delete(_musicFileWithoutALyricsFile);
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
                if (File.Exists(_musicFileWithoutALyricsFile))
                    File.Delete(_musicFileWithoutALyricsFile);
            }
            catch
            {
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid path")]
        public void ReaderTestsInvalidPathNotDelete()
        {
            IndexingFilesProcessor _fileProcessor = new IndexingFilesProcessor();
            Reader reader = new Reader(ReaderMocks.MockFileActivityInvalidPath(@"c:\users\something\something",RecordNegativeActivity));
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            try
            {
                reader.ParsePath(@"c:\users\something\something", "*.mp3|*.ogg");

            }
            catch(Exception ex)
            {

                throw ex;
            }
        }

        private void RecordNegativeActivity(string path, ActivityStatus activityStatus)
        {
            Assert.AreNotEqual(ActivityStatus.None, activityStatus);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReaderTestsNoPath()
        {
            IndexingFilesProcessor _fileProcessor = new IndexingFilesProcessor();
            Reader reader = new Reader(null);
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(null, "*.mp3|*.ogg");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReaderTestsNoSearchPattern()
        {
            IndexingFilesProcessor _fileProcessor = new IndexingFilesProcessor();
            Reader reader = new Reader(null);
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(@"c:\users\something\something", "");
        }

        [Test]
        public void ReaderTestsDeletingPath()
        {
            Reader reader = new Reader(ReaderMocks.MockFileActivityDelete(@"c:\users\something\something",RecordNegativeActivity));
            reader.ProcessFiles = ProcessFiles;
            reader.ParsePath(@"c:\users\something\something", "*.mp3|*.ogg",ProcessType.Delete);
        }

        private void ProcessFiles(IEnumerable<string> paths)
        {
            Assert.True(paths.Any());
        }

        [Test]
        public void ReaderMusicTestsFolderOk()
        {
            Reader reader = new Reader(ReaderMocks.MockFileActivityProcessFolder(@"TestData\Music",RecordNegativeActivity));
            reader.ProcessFiles = ProcessFiles;
            reader.ParsePath(@"TestData\Music", "*.mp3|*.ogg");
        }

        [Test]
        public void ReaderMusicTestsFileOk()
        {
            Reader reader = new Reader(ReaderMocks.MockFileActivityProcessFile(@"TestData\Music\MockMp3.mp3",RecordNegativeActivity));
            reader.ProcessFiles = ProcessFiles;
            reader.ParsePath(@"TestData\Music\MockMp3.mp3", "*.mp3|*.ogg");
        }

        //[Test]
        //public void ReaderMusicToLyricsTestsFolderOkSomethingNew()
        //{
        //    File.Create(_newFileDuringTests);
        //    MockMusicToLyricsFilesProcessor _fileProcessor = new MockMusicToLyricsFilesProcessor();
        //    Reader reader = new Reader(null);
        //    reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
        //    reader.ParsePath(@"TestData\Music\Sub", "*.mp3|*.ogg");
        //    Assert.AreEqual(1, _fileProcessor.Counter);
        //}

        //[Test]
        //public void ReaderMusicToLyricsTestsFileOk()
        //{
        //    MockMusicToLyricsFilesProcessor _fileProcessor = new MockMusicToLyricsFilesProcessor();
        //    Reader reader = new Reader(null);
        //    _fileProcessor.RetryExisting = true;
        //    reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
        //    reader.ParsePath(@"TestData\Music\MockMp3.mp3", "*.mp3|*.ogg");
        //    Assert.AreEqual(1, _fileProcessor.Counter);
        //}

        //[Test]
        //public void ReaderMusicToLyricsTestsFileNoLyricsFound()
        //{
        //    MockMusicToLyricsFilesProcessor _fileProcessor = new MockMusicToLyricsFilesProcessor();
        //    Reader reader = new Reader(null);
        //    _fileProcessor.RetryExisting = true;
        //    reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
        //    reader.ParsePath(@"TestData\Music\Sub\MockOgg.ogg", "*.mp3|*.ogg");
        //    Assert.AreEqual(1, _fileProcessor.Counter);
        //}

        //[Test]
        //public void ReaderMusicToLyricsTestsFileNeedsPostProcessing()
        //{
        //    File.Create(_newFileDuringTestsNeedsProcessing);
        //    MockMusicToLyricsFilesProcessor _fileProcessor = new MockMusicToLyricsFilesProcessor();
        //    Reader reader = new Reader(null);
        //    _fileProcessor.RetryExisting = true;
        //    reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
        //    reader.ParsePath(_newFileDuringTestsNeedsProcessing, "*.mp3|*.ogg");
        //    Assert.AreEqual(1, _fileProcessor.Counter);
        //}
    }
}
