using Sciendo.Index.Solr;
using Sciendo.Indexer.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Index.Tests
{
    public class MockLyricsFilesProcessor:LyricsFilesProcessor
    {
        public MockLyricsFilesProcessor(string musicRootFolder):base(musicRootFolder,new MockSender(), new MockLyricsDeserializer())
        {

        }
    }
}
