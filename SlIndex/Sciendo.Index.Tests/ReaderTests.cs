using Newtonsoft.Json;
using NUnit.Framework;
using Sciendo.Index.Solr;
using Sciendo.Indexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Index.Tests
{
    [TestFixture]
    public class ReaderTests
    {
        private MusicFilesProcessor _fileProcessor = new MusicFilesProcessor();

        [Test]
        public void ReaderMusicTestsOk()
        {
            Reader reader = new Reader();
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParseDirectory(@"c:\users\octo\music","*.mp3|*.ogg");
            Console.WriteLine(_fileProcessor.Counter);
        }
        [Test]
        public void ReaderLyricsTestsOk()
        {
            LyricsFilesProcessor processor = new LyricsFilesProcessor(@"c:\users\octo\music");
            Reader reader = new Reader();
            reader.ProcessFiles = processor.ProcessFilesBatch;
            reader.ParseDirectory(@"C:\Code\Sciendo\Sciendo.RESTl\target", "*.lrc");
            Console.WriteLine(processor.Counter);
        }
        [Test]
        public void ReaderMusicTestsForWronglyFormattedStringsOk()
        {
            Reader reader = new Reader();
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParseDirectory(@"c:\users\octo\music\p\paul_oakenfoald", "*.mp3|*.ogg");
            Console.WriteLine(_fileProcessor.Counter);
        }

    }
}
