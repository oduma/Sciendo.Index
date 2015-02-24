using System;
using System.Collections.Generic;
using System.IO;
using Id3;
using NUnit.Framework;
using Rhino.Mocks;
using Sciendo.Lyrics.Common;
using Sciendo.Lyrics.Provider.Service;

namespace Sciendo.Lyrics.Provider.Tests
{
    [TestFixture]
    public class PipelineTests
    {
        public WebDownloaderBase MockWebDownloader(string url)
        {
            var mockWebDownloader = MockRepository.GenerateStub<WebDownloaderBase>();
            mockWebDownloader.Stub(m => m.TryQuery<string>(url)).Return(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<LyricsResult>
  <artist>Tool</artist>
  <song>Schism</song>
  <lyrics>
    I know the pieces fit 'cause I watched them fall away Mildewed and smoldering, fundamental differing Pure intention juxtaposed will set two lover's souls in motion Disintegrating [...]
  </lyrics>
  <url>http://lyrics.wikia.com/Tool:Schism</url>
  <page_namespace>0</page_namespace>
  <page_id>171909</page_id>
  <isOnTakedownList>0</isOnTakedownList>
</LyricsResult>");
            return mockWebDownloader;
        }

        [Test]
        public void ConfigurePipeline_NoExecutionContextFile()
        {
            var pipeline= new Pipeline("source", "target",MockWebDownloader(""),"");
            Assert.IsNotNull(pipeline.ExecutionContext);
        }

        [Test]
        [ExpectedException(typeof(NoExecutionContextException))]
        public void ConfigurePipelineExecutionContextNotCreated()
        {
            var pipeline = new Pipeline("source", "", MockWebDownloader(""), "abc.xml");
        }

        [Test]
        public void ConfigurePipeline_LoadExecutionFile()
        {
            ExecutionContext executionContext = new ExecutionContext
            {
                SourceRootDirectory = "source",
                TargetRootDirectory = "target",
                ReadWrites =
                    new List<ReadWriteContext>
                    {
                        new ReadWriteContext
                        {
                            ReadLocation = "source\\dir1\\dir2\\nofile.txt",
                            Status = Status.FileNotFound
                        },
                        new ReadWriteContext
                        {
                            artist = "my artist",
                            song = "my song",
                            ReadLocation = "source\\dir1\\dir2\\file.txt",
                            Status = Status.ArtistSongRetrievedFromFile
                        }
                    }
            };
            Sciendo.Common.Serialization.Serializer.SerializeOneToFile(executionContext,"executioncontext.xml");

            var pl = new Pipeline("", "", MockWebDownloader(""), "executioncontext.xml");
            Assert.AreEqual("source", pl.ExecutionContext.SourceRootDirectory);
            Assert.AreEqual("target", pl.ExecutionContext.TargetRootDirectory);
            Assert.IsNotNull(pl.ExecutionContext.ReadWrites);
            Assert.AreEqual(2,executionContext.ReadWrites.Count);
        }

        [Test]
        [ExpectedException(typeof(NoExecutionContextException))]
        public void ContinueProcessing_NoExecutionContext()
        {
            var pl = new Pipeline("source", "target", MockWebDownloader(""), "abc");
            pl.ExecutionContext = null;
            pl.ContinueProcessing(true,null,null, new LyricsDeserializer());
        }

    }
}
