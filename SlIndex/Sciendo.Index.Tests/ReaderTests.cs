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
        private FilesProcessor _fileProcessor = new FilesProcessor(@"c:\users\octo\music");

        [Test]
        public void ReaderTestsOk()
        {
            MusicReader reader = new MusicReader();
            reader.RootDirectory = @"c:\users\octo\music";
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.RootReader();
            Console.WriteLine(_fileProcessor.Counter);
        }
    }
}
