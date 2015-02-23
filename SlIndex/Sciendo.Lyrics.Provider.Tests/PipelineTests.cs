using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sciendo.Lyrics.Common;
using Sciendo.Lyrics.Provider.Service;

namespace Sciendo.Lyrics.Provider.Tests
{
    [TestFixture]
    public class PipelineTests
    {
        [Test]
        public void ConfigurePipeline_NoExecutionContextFile()
        {
            var pipeline= new Pipeline("source", "target","");
            Assert.IsNotNull(pipeline.ExecutionContext);
        }

        [Test]
        [ExpectedException(typeof(NoExecutionContextException))]
        public void ConfigurePipelineExecutionContextNotCreated()
        {
            var pipeline = new Pipeline("source", "", "abc.xml");
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

            var pl = new Pipeline("", "", "executioncontext.xml");
            Assert.AreEqual("source", pl.ExecutionContext.SourceRootDirectory);
            Assert.AreEqual("target", pl.ExecutionContext.TargetRootDirectory);
            Assert.IsNotNull(pl.ExecutionContext.ReadWrites);
            Assert.AreEqual(2,executionContext.ReadWrites.Count);
        }

        [Test]
        [ExpectedException(typeof (Exception), ExpectedMessage = "Execution Context not established.")]
        public void ContinueProcessing_NoExecutionContext()
        {
            var pl = new Pipeline("source","target","abc");
            pl.ExecutionContext = null;
            pl.ContinueProcessing(true,null,null);
        }

    }
}
