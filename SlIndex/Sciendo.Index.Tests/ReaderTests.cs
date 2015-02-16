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
        private MusicFilesProcessor _fileProcessor = new MusicFilesProcessor(new MockSender());

        [Test]
        [ExpectedException(typeof(ArgumentException),ExpectedMessage="Invalid path")]
        public void ReaderTestsInvalidPath()
        {
            Reader reader = new Reader(null);
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(@"c:\users\something\something", "*.mp3|*.ogg");
        }

        [Test]
        public void ReaderMusicTestsOk()
        {
            Reader reader = new Reader(null);
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(@"TestData\Music", "*.mp3|*.ogg");
            Console.WriteLine(_fileProcessor.Counter);
        }
        [Test]
        public void ReaderLyricsTestsOk()
        {
            LyricsFilesProcessor processor = new LyricsFilesProcessor(@"c:\users\octo\music", new MockSender());
            Reader reader = new Reader(null);
            reader.ProcessFiles = processor.ProcessFilesBatch;
            reader.ParsePath(@"C:\Code\Sciendo\Sciendo.RESTl\target", "*.lrc");
            Console.WriteLine(processor.Counter);
        }
        [Test]
        public void ReaderMusicTestsForWronglyFormattedStringsOk()
        {
            Reader reader = new Reader(null);
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParsePath(@"c:\users\octo\music\p\paul_oakenfoald", "*.mp3|*.ogg");
            Console.WriteLine(_fileProcessor.Counter);
        }

    }
}
