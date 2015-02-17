using Sciendo.Index.Solr;
using Sciendo.Indexer.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Index.Tests
{
    public class MockMusicFilesProcessor:FilesProcessor
    {

        protected override IEnumerable<Document> PrepareDocuments(IEnumerable<string> files, string rootFolder)
        {
            yield return new FullDocument(files.First(), rootFolder, new string[] { "test artist" }, "test song", "test alubm");
        }
    }
}
