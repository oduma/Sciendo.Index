using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Sciendo.Common.Serialization;
using Sciendo.Lyrics.Common;

namespace Sciendo.Lyrics.Provider.Tests
{
    [TestFixture]
    public class LyricsCommonTests
    {
        [Test]
        [ExpectedException(typeof(FileNotFoundException), ExpectedMessage = "nosuchfile.xml")]
        public void LyricsDeserializerNoFile()
        {
            LyricsDeserializer lyrics= new LyricsDeserializer();
            Assert.IsNull(lyrics.DeserializeFromFile<LyricsResult>("nosuchfile.xml"));
        }
        [Test]
        [ExpectedException(typeof(PreSerializationCheckException))]
        public void LyricsDeserializerFileNoLyrics()
        {
            LyricsDeserializer lyrics = new LyricsDeserializer();
            Assert.IsNull(lyrics.DeserializeFromFile<LyricsResult>(@"TestData\ExampleNoLyrics.xml"));
        }
        [Test]
        [ExpectedException(typeof(PreSerializationCheckException))]
        public void LyricsDeserializerFileWrongFromat()
        {
            LyricsDeserializer lyrics = new LyricsDeserializer();
            Assert.IsNull(lyrics.DeserializeFromFile<LyricsResult>(@"TestData\ExampleWrongFormat.xml"));
        }
        [Test]
        public void LyricsDeserializerFileLyricsWithProcessing()
        {
            LyricsDeserializer lyrics = new LyricsDeserializer();
            var results = lyrics.DeserializeFromFile<LyricsResult>(@"TestData\ExampleNeedsPostProcessing.xml");
            Assert.IsNotNull(results);
            Assert.AreEqual("\n    I know the pieces fit 'cause I watched them fall away Mildewed and smoldering, fundamental differing Pure intention juxtaposed will set two lover's souls in motion Disintegrating [...]\n  ", results.lyrics);
        }
        [Test]
        public void LyricsDeserializerFileWithLyricsNoProcessing()
        {
            LyricsDeserializer lyrics = new LyricsDeserializer();
            var results = lyrics.DeserializeFromFile<LyricsResult>(@"TestData\Example.xml");
            Assert.IsNotNull(results);
            Assert.AreEqual("\n    I know the pieces fit 'cause I watched them fall away Mildewed and smoldering, fundamental differing Pure intention juxtaposed will set two lover's souls in motion Disintegrating [...]\n  ", results.lyrics);
        }

    }

}
