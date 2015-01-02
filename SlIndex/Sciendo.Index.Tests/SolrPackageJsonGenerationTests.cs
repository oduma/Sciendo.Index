using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Sciendo.Index.Solr;

namespace Sciendo.Index.Tests
{
    [TestFixture]
    public class SolrPackageJsonGenerationTests
    {
        [Test]
        public void GenerateInitialPackageOk()
        {
            Document doc = new Document("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.wav",
                "C:\\Users\\octo\\Music", new[] {"Accept"}, "The Hungry Years", "Fast As a Shark",
                "[Intro:]\n\"Hei--di, heido, heida\nHei--di, heido, heida\nHeidi, heido, heidahahahahahahaha...\"\n\n[Scratching sounds:]\n\n[Udo screaming:]\n\n[Fast As A Shark begins:]\n\nFog in the streets\nA[...]");
            var actual= JsonConvert.SerializeObject(doc);

            Document actualDoc = JsonConvert.DeserializeObject<Document>(actual);

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
            Document doc = new Document("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.wav",
                "C:\\Users\\octo\\Music", new[] { "Accept" }, "The Hungry Years", "Fast As a Shark",
                "[Intro:]\n\"Hei--di, heido, heida\nHei--di, heido, heida\nHeidi, heido, heidahahahahahahaha...\"\n\n[Scratching sounds:]\n\n[Udo screaming:]\n\n[Fast As A Shark begins:]\n\nFog in the streets\nA[...]");

            Package package = new Package {add = new PackageContent {doc = doc}};
            SolrSender.Send("http://localhost:8090/solr/update/json",package);
        }
    }
}
