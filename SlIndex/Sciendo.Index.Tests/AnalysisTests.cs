using NUnit.Framework;
using Sciendo.Music.Agent.Service;
using Sciendo.Music.Contracts.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Sciendo.Music.Tests
{
    [TestFixture]
    public class AnalysisTests
    {
        [Test]
        public void GetAllSnapshotsOk()
        {
            AnalysisService analysisService = new AnalysisService("");
            var actual = analysisService.GetAllAnalysisSnaphots();
            Assert.IsNotNull(actual);
            Assert.Greater(actual.Length,1);
        }
        [Test]
        public void GetElementsBySnapshotOk()
        {
            AnalysisService analysisService = new AnalysisService("");
            var actual = analysisService.GetAnalysis("leaf", 3);
            Assert.IsNotNull(actual);
            Assert.Greater(actual.Length, 1);
        }

        [Test]
        public void CreateSnapshotOk()
        {
            using(TransactionScope transactionScope= new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                AnalysisService analysisService = new AnalysisService("");
                var actual = analysisService.CreateNewSnapshot("test");
                Assert.IsNotNull(actual);
                Assert.Greater(actual.SnapshotId, 1);
                analysisService = new AnalysisService("");
                var actualsnapshots = analysisService.GetAllAnalysisSnaphots();
                Assert.AreEqual(4, actualsnapshots.Length);
            }
        }
        [Test]
        public void CreateElementsOk()
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                AnalysisService analysisService = new AnalysisService("");
                var actual = analysisService.CreateElements(new Element[] { new Element { IsIndexed = false, LyricsFileFlag = LyricsFileFlag.HasLyricsFile | LyricsFileFlag.LyricsFileOk, MusicFileFlag = MusicFileFlag.HasTag | MusicFileFlag.IsMusicFile | MusicFileFlag.HasArtistTag, Name = "test element one", SnapshotId = 3 } });
                Assert.IsNotNull(actual);
                Assert.AreEqual(1, actual);
            }

        }
    }
}
