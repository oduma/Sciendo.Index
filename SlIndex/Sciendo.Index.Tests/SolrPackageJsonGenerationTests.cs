using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;

namespace Sciendo.Index.Tests
{
    [TestFixture]
    public class SolrPackageJsonGenerationTests
    {
        [Test]
        public void GenerateInitialPackageOk()
        {
            FullDocument doc = new FullDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.wav",
                "C:\\Users\\octo\\Music", new[] {"Accept"}, "The Hungry Years", "Fast As a Shark",
                "[Intro:]\n\"Hei--di, heido, heida\nHei--di, heido, heida\nHeidi, heido, heidahahahahahahaha...\"\n\n[Scratching sounds:]\n\n[Udo screaming:]\n\n[Fast As A Shark begins:]\n\nFog in the streets\nA[...]");
            var actual= JsonConvert.SerializeObject(doc);

            FullDocument actualDoc = JsonConvert.DeserializeObject<FullDocument>(actual);

            Assert.AreEqual(doc.album.set, actualDoc.album.set);
            Assert.AreEqual(doc.artist.set, actualDoc.artist.set);
            Assert.AreEqual(doc.file_path_id, actualDoc.file_path_id);
            Assert.AreEqual(doc.extension_f.set, actualDoc.extension_f.set);
            Assert.AreEqual(doc.file_path.set, actualDoc.file_path.set);
            Assert.AreEqual(doc.letter_catalog_f.set, actualDoc.letter_catalog_f.set);
            Assert.AreEqual(doc.lyrics.set, actualDoc.lyrics.set);
            Assert.AreEqual(doc.title.set, actualDoc.title.set);

        }
        [Test]
        public void SendAPackageToSolrOk()
        {
            FullDocument doc1 = new FullDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.wav",
                "C:\\Users\\octo\\Music", new[] { "Accept" }, "The Hungry Years", "Fast As a Shark",
                "[Intro:]\n\"Hei--di, heido, heida\nHei--di, heido, heida\nHeidi, heido, heidahahahahahahaha...\"\n\n[Scratching sounds:]\n\n[Udo screaming:]\n\n[Fast As A Shark begins:]\n\nFog in the streets\nA[...]");

            FullDocument doc2 = new FullDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.abc",
                "C:\\Users\\octo\\Music", new[] { "Accept" }, "The Hungry Years", "Fast As a Shark",
                "[Intro:]\n\"Hei--di, heido, heida\nHei--di, heido, heida\nHeidi, heido, heidahahahahahahaha...\"\n\n[Scratching sounds:]\n\n[Udo screaming:]\n\n[Fast As A Shark begins:]\n\nFog in the streets\nA[...]");

            FullDocument[] package = new FullDocument[] { doc1 ,doc2};
            var response = SolrSender.TrySend("http://localhost:8080/solr/medialib/update/json?commitWithin=1000", package);
            //SolrSender.TrySend("http://localhost:8080/solr/medialib/update/json", new CommitWithin(1000));
            Assert.True(response.Status==Status.Done);
            Assert.Greater(response.Time, 0);
        }
        [Test]
        public void SendAPackageToSolrPartiallyOk()
        {
            FullDocument doc1 = new FullDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.new",
                "C:\\Users\\octo\\Music", new[] { "Accept" }, "The Hungry Years", "Fast As a Shark",
                "[Intro:]\n\"Hei--di, heido, heida\nHei--di, heido, heida\nHeidi, heido, heidahahahahahahaha...\"\n\n[Scratching sounds:]\n\n[Udo screaming:]\n\n[Fast As A Shark begins:]\n\nFog in the streets\nA[...]");

            FullDocument doc2 = new FullDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.abc",
                "C:\\Users\\octo\\Music", new[] { "Accept" }, "The Hungry Years", "Fast As a Shark",
                "[Intro:]\n\"Hei--di, heido, heida\nHei--di, heido, heida\nHeidi, heido, heidahahahahahahaha...\"\n\n[Scratching sounds:]\n\n[Udo screaming:]\n\n[Fast As A Shark begins:]\n\nFog in the streets\nA[...]");

            doc2.file_path = null;
            FullDocument[] package = new FullDocument[] { doc1, doc2 };
            var response = SolrSender.TrySend("http://localhost:8090/solr/medialib/update?wt=json", package);
            Assert.True(response.Status == Status.NotIndexed);
            Assert.AreEqual(response.Time, 0);
        }
    }
}
