using Newtonsoft.Json;
using NUnit.Framework;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Contracts.Solr;
using Sciendo.Music.Real.Solr;

namespace Sciendo.Music.Tests
{
    [TestFixture]
    public class SolrPackageJsonGenerationTests
    {
        [Test]
        public void GenerateASetPackageOk()
        {
            FullDocument doc = new FullDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.wav",
                "C:\\Users\\octo\\Music", new[] {"Accept"}, "The Hungry Years", "Fast As a Shark",
                "[Intro:]\n\"Hei--di, heido, heida\nHei--di, heido, heida\nHeidi, heido, heidahahahahahahaha...\"\n\n[Scratching sounds:]\n\n[Udo screaming:]\n\n[Fast As A Shark begins:]\n\nFog in the streets\nA[...]");
            var actual= JsonConvert.SerializeObject(doc);

            FullDocument actualDoc = JsonConvert.DeserializeObject<FullDocument>(actual);

            Assert.AreEqual(doc.Album.Set, actualDoc.Album.Set);
            Assert.AreEqual(doc.Artist.Set, actualDoc.Artist.Set);
            Assert.AreEqual(doc.FilePathId, actualDoc.FilePathId);
            Assert.AreEqual(doc.ExtensionF.Set, actualDoc.ExtensionF.Set);
            Assert.AreEqual(doc.file_path.Set, actualDoc.file_path.Set);
            Assert.AreEqual(doc.LetterCatalogF.Set, actualDoc.LetterCatalogF.Set);
            Assert.AreEqual(doc.Lyrics.Set, actualDoc.Lyrics.Set);
            Assert.AreEqual(doc.Title.Set, actualDoc.Title.Set);

        }

        [Test]
        public void GenerateADeletePackageOk()
        {
            DeleteDocument doc =
                new DeleteDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.wav");
            var actual = JsonConvert.SerializeObject(doc);

            DeleteDocument actualDoc = JsonConvert.DeserializeObject<DeleteDocument>(actual);

            Assert.AreEqual(doc.DeleteById.Id, actualDoc.DeleteById.Id);
        }

        [Test]
        [Ignore("Integration test needs solr in order to run.")]
        public void SendADeletePackageToSolrOk()
        {
            FullDocument updateDoc1 = new FullDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.wav",
                "C:\\Users\\octo\\Music", new[] { "Accept" }, "The Hungry Years", "Fast As a Shark",
                "[Intro:]\n\"Hei--di, heido, heida\nHei--di, heido, heida\nHeidi, heido, heidahahahahahahaha...\"\n\n[Scratching sounds:]\n\n[Udo screaming:]\n\n[Fast As A Shark begins:]\n\nFog in the streets\nA[...]");

            FullDocument updateDoc2 = new FullDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.def",
    "C:\\Users\\octo\\Music", new[] { "Accept" }, "The Hungry Years", "Fast As a Shark",
    "[Intro:]\n\"Hei--di, heido, heida\nHei--di, heido, heida\nHeidi, heido, heidahahahahahahaha...\"\n\n[Scratching sounds:]\n\n[Udo screaming:]\n\n[Fast As A Shark begins:]\n\nFog in the streets\nA[...]");


            FullDocument[] package = new FullDocument[] { updateDoc1,updateDoc2 };

            var sentPackage1 = JsonConvert.SerializeObject(package);
            SolrSender solrSender = new SolrSender("http://localhost:8090/solr/medialib/update/json?commitWithin=1000");
            var response = solrSender.TrySend(package);
            //SolrSender.TrySend("http://localhost:8090/solr/medialib/update/json", new CommitWithin(1000));
            Assert.True(response.Status == Status.Done);
            Assert.Greater(response.Time, 0);

            DeleteDocument doc1 = new DeleteDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.wav");

            DeleteDocument doc2 = new DeleteDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.def");

            solrSender = new SolrSender("http://localhost:8090/solr/medialib/update/json?commitWithin=1000");
            response = solrSender.TrySend(doc1);
            Assert.True(response.Status == Status.Done);
            Assert.GreaterOrEqual(response.Time, 0);
            response = solrSender.TrySend(doc2);
            Assert.True(response.Status == Status.Done);
            Assert.GreaterOrEqual(response.Time, 0);
            response = solrSender.TrySend(new Commit());
            Assert.True(response.Status == Status.Done);
            Assert.GreaterOrEqual(response.Time, 0);

        }


        [Test]
        [Ignore("Integration test needs solr in order to run.")]
        public void SendAPackageToSolrOk()
        {
            FullDocument doc1 = new FullDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.wav",
                "C:\\Users\\octo\\Music", new[] { "Accept" }, "The Hungry Years", "Fast As a Shark",
                "[Intro:]\n\"Hei--di, heido, heida\nHei--di, heido, heida\nHeidi, heido, heidahahahahahahaha...\"\n\n[Scratching sounds:]\n\n[Udo screaming:]\n\n[Fast As A Shark begins:]\n\nFog in the streets\nA[...]");

            FullDocument doc2 = new FullDocument("C:\\Users\\octo\\Music\\a\\Accept\\The Hungry Years\\Fast As a Shark.abc",
                "C:\\Users\\octo\\Music", new[] { "Accept" }, "The Hungry Years", "Fast As a Shark",
                "[Intro:]\n\"Hei--di, heido, heida\nHei--di, heido, heida\nHeidi, heido, heidahahahahahahaha...\"\n\n[Scratching sounds:]\n\n[Udo screaming:]\n\n[Fast As A Shark begins:]\n\nFog in the streets\nA[...]");

            FullDocument[] package = new FullDocument[] { doc1 ,doc2};
            SolrSender solrSender = new SolrSender("http://localhost:8090/solr/medialib/update/json?commitWithin=1000");
            var response = solrSender.TrySend(package);
            //SolrSender.TrySend("http://localhost:8080/solr/medialib/update/json", new CommitWithin(1000));
            Assert.True(response.Status==Status.Done);
            Assert.Greater(response.Time, 0);
        }

        [Test]
        [Ignore("Integration test needs solr in order to run.")]
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
            SolrSender solrSender = new SolrSender("http://localhost:8090/solr/medialib/update/json?commitWithin=1000");
            var response = solrSender.TrySend(package);
            Assert.True(response.Status == Status.Error);
            Assert.AreEqual(response.Time, 0);
        }
    }
}
