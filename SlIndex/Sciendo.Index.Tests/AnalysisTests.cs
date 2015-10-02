using NUnit.Framework;
using Sciendo.Music.Agent.Service;
using Sciendo.Music.Contracts.Analysis;
using Sciendo.Music.Real.Analysis;
using Sciendo.Music.Real.Feedback;
using Sciendo.Music.Solr.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            AnalysisService analysisService = new AnalysisService("","","",null,null);
            var actual = analysisService.GetAllAnalysisSnaphots();
            Assert.IsNotNull(actual);
            Assert.Greater(actual.Length,1);
        }
        [Test]
        public void GetElementsBySnapshotOk()
        {
            AnalysisService analysisService = new AnalysisService("","","",null,null);
            var actual = analysisService.GetAnalysis("leaf", 3);
            Assert.IsNotNull(actual);
            Assert.Greater(actual.Length, 1);
        }

        [Test]
        public void CreateSnapshotOk()
        {
            using(TransactionScope transactionScope= new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                AnalysisService analysisService = new AnalysisService("","","",null,null);
                var actual = analysisService.CreateNewSnapshot("test");
                Assert.IsNotNull(actual);
                Assert.Greater(actual.SnapshotId, 1);
                analysisService = new AnalysisService("","","",null,null);
                var actualsnapshots = analysisService.GetAllAnalysisSnaphots();
                Assert.AreEqual(4, actualsnapshots.Length);
            }
        }
        [Test]
        public void CreateElementsOk()
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                AnalysisService analysisService = new AnalysisService("","","",null,null);
                var actual = analysisService.CreateElements(new Element[] { new Element {IndexedFlag = IndexedFlag.NotIndexed, LyricsFileFlag = LyricsFileFlag.HasLyricsFile | LyricsFileFlag.LyricsFileOk, MusicFileFlag = MusicFileFlag.HasTag | MusicFileFlag.IsMusicFile | MusicFileFlag.HasArtistTag, Name = "test element one", SnapshotId = 3 } });
                Assert.IsNotNull(actual);
                Assert.AreEqual(1, actual);
            }

        }

        [Test]
        public void GetStatisticsFromRoot_Ok()
        {
            AnalysisService svc = new AnalysisService("trunk", "", "", null,null);
            var actual = svc.GetStatistics(null, 3);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
        }

        [Test]
        public void AnalyseThisFromRoot()
        {
            IAnalysis svc = ReaderMocks.MockAnalysisServiceForAnalyseThis(@"TestData\Music", 
@"TestData\Lyrics", "*.mp3|*.ogg", 23, recordStatisticsActivity);
            svc.AnaliseThis("", 23);
        }

        private void recordStatisticsActivity(int snapshotId, Real.Feedback.ActivityStatus activityStatus)
        {
            Assert.AreEqual(23, snapshotId);
            Assert.AreNotEqual(ActivityStatus.None, activityStatus);
        }

        //[Test]
        //public void AnalysisUtilsGetMusciFileFlagNoMusicFileTest()
        //{
        //    var utils = new Utils();
        //    Assert.AreEqual(MusicFileFlag.NotAMusicFile, utils.GetMusicFlag("something.else", "*.mp3|*.ogg"));
        //}
        //[Test]
        //public void AnalysisUtilsGetMusciFileFlagMusicFileNoTagTest()
        //{
        //    var utils = new Utils();
        //    Assert.AreEqual(MusicFileFlag.IsMusicFile, utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_NoTag));
        //}
        //[Test]
        //public void AnalysisUtilsGetMusciFileFlagMusicFileUnknownTagTest()
        //{
        //    var utils = new Utils();

        //    Assert.AreEqual(MusicFileFlag.IsMusicFile, utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_UnknownTag));
        //}
        //[Test]
        //public void AnalysisUtilsGetMusciFileFlagMusicFileNoTagTest1()
        //{
        //    var utils = new Utils();

        //    Assert.AreEqual(MusicFileFlag.IsMusicFile, utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_NoTag1));
        //}
        //[Test]
        //public void AnalysisUtilsGetMusciFileFlagMusicFileErrorTagTest()
        //{
        //    var utils = new Utils();

        //    Assert.AreEqual(MusicFileFlag.IsMusicFile, utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_ErrorTag));
        //}
        //[Test]
        //public void AnalysisUtilsGetMusciFileFlagMusicFileEmptyTagTest()
        //{
        //    var utils = new Utils();

        //    Assert.AreEqual(MusicFileFlag.IsMusicFile, utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_EmptyTag));
        //}
        //[Test]
        //public void AnalysisUtilsGetMusciFileFlagMusicFileNoArtistTest()
        //{
        //    var utils = new Utils();

        //    Assert.AreEqual(MusicFileFlag.IsMusicFile|MusicFileFlag.HasAlbumTag|MusicFileFlag.HasTitleTag|MusicFileFlag.HasTag, utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_EverythingButArtist));
        //}
        //[Test]
        //public void AnalysisUtilsGetMusciFileFlagMusicFileNoAlbumTest()
        //{
        //    var utils = new Utils();

        //    Assert.AreEqual(MusicFileFlag.IsMusicFile | MusicFileFlag.HasArtistTag | MusicFileFlag.HasTitleTag | MusicFileFlag.HasTag, utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_TagOk));
        //}
        //[Test]
        //public void AnalysisUtilsGetMusciFileFlagMusicFileNoTitleTest()
        //{
        //    var utils = new Utils();

        //    Assert.AreEqual(MusicFileFlag.IsMusicFile | MusicFileFlag.HasArtistTag | MusicFileFlag.HasAlbumTag | MusicFileFlag.HasTag, utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_EverythingButTitle));
        //}
        //[Test]
        //public void AnalysisUtilsGetMusciFileFlagMusicFileFullTagTest()
        //{
        //    var utils = new Utils();

        //    Assert.AreEqual(MusicFileFlag.IsMusicFile | MusicFileFlag.HasArtistTag | MusicFileFlag.HasAlbumTag | MusicFileFlag.HasTag | MusicFileFlag.HasTitleTag, utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_FullTag));
        //}

        [Test]
        public void AnalysisUtilsLyricsFileFlagNoFileTest1()
        {
            var utils = new Utils(null);

            Assert.AreEqual(LyricsFileFlag.NoLyricsFile, utils.GetLyricsFlag("something.else", "*.mp3|*.ogg", "", ""));
        }
        [Test]
        public void AnalysisUtilsLyricsFileFlagNoFileTest2()
        {
            var utils = new Utils(null);

            Assert.AreEqual(LyricsFileFlag.NoLyricsFile, utils.GetLyricsFlag(@"TestData\Music\Sub1\MockOggWithoutLyrics.ogg", "*.mp3|*.ogg", @"TestData\Music", @"TestData\Lyrics"));
        }
        [Test]
        public void AnalysisUtilsLyricsFileFlagLyricsNotFoundTest()
        {
            var utils = new Utils(null);

            Assert.AreEqual(LyricsFileFlag.HasLyricsFile | LyricsFileFlag.LyricsFileNoLyrics, utils.GetLyricsFlag(@"TestData\Music\Sub1\MockOggLyricsNotFound.ogg", "*.mp3|*.ogg", @"TestData\Music", @"TestData\Lyrics"));
        }
        [Test]
        public void AnalysisUtilsLyricsFileFlagLyricsWithErrorTest()
        {
            var utils = new Utils(null);

            Assert.AreEqual(LyricsFileFlag.HasLyricsFile | LyricsFileFlag.LyricsFileWithError, utils.GetLyricsFlag(@"TestData\Music\Sub1\MockOggWithErrorLyrics.ogg", "*.mp3|*.ogg", @"TestData\Music", @"TestData\Lyrics"));
        }
        [Test]
        public void AnalysisUtilsLyricsFileFlagOkTest()
        {
            var utils = new Utils(null);

            Assert.AreEqual(LyricsFileFlag.HasLyricsFile | LyricsFileFlag.LyricsFileOk, utils.GetLyricsFlag(@"TestData\Music\Sub1\MockOggWithLyrics.ogg", "*.mp3|*.ogg", @"TestData\Music", @"TestData\Lyrics"));
        }

        [Test]
        public void AnalysisUtilsIndexedFlagNotIndexedTest1()
        {
            var utils = new Utils(SolrResultsProviderMocks.GetExceptionMock("something.else"));

            Assert.AreEqual(IndexedFlag.NotIndexed, utils.GetIndexedFlag("something.else"));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagNotIndexedTest2()
        {
            var utils = new Utils(SolrResultsProviderMocks.GetNullResultsMock("something.else"));

            Assert.AreEqual(IndexedFlag.NotIndexed, utils.GetIndexedFlag("something.else"));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagNotIndexedTest3()
        {
            var utils = new Utils(SolrResultsProviderMocks.GetNullResultRowsMock("something.else"));

            Assert.AreEqual(IndexedFlag.NotIndexed, utils.GetIndexedFlag("something.else"));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagNotIndexedTest4()
        {
            var utils = new Utils(SolrResultsProviderMocks.GetNoResultRowsMock("something.else"));

            Assert.AreEqual(IndexedFlag.NotIndexed, utils.GetIndexedFlag("something.else"));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagNotIndexedTest5()
        {
            var utils = new Utils(SolrResultsProviderMocks.GetOneEmptyResultRowsMock("something.else"));

            Assert.AreEqual(IndexedFlag.NotIndexed, utils.GetIndexedFlag("something.else"));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagIndexedEverythingButTheAlbumTest()
        {
            var utils = new Utils(SolrResultsProviderMocks.GetIndexedResultMock("something.else",MissingType.Album));

            Assert.AreEqual(IndexedFlag.Indexed|IndexedFlag.IndexedArtist|IndexedFlag.IndexedLyrics|IndexedFlag.IndexedTitle, utils.GetIndexedFlag("something.else"));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagIndexedEverythingButTheArtistTest()
        {
            var utils = new Utils(SolrResultsProviderMocks.GetIndexedResultMock("something.else", MissingType.Artist));

            Assert.AreEqual(IndexedFlag.Indexed | IndexedFlag.IndexedAlbum | IndexedFlag.IndexedLyrics | IndexedFlag.IndexedTitle, utils.GetIndexedFlag("something.else"));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagIndexedEverythingButTheTitleTest()
        {
            var utils = new Utils(SolrResultsProviderMocks.GetIndexedResultMock("something.else", MissingType.Title));

            Assert.AreEqual(IndexedFlag.Indexed | IndexedFlag.IndexedArtist | IndexedFlag.IndexedLyrics | IndexedFlag.IndexedAlbum, utils.GetIndexedFlag("something.else"));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagIndexedEverythingButTheLyricsTest()
        {
            var utils = new Utils(SolrResultsProviderMocks.GetIndexedResultMock("something.else", MissingType.Lyrics));

            Assert.AreEqual(IndexedFlag.Indexed | IndexedFlag.IndexedArtist | IndexedFlag.IndexedAlbum | IndexedFlag.IndexedTitle, utils.GetIndexedFlag("something.else"));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagIndexedOnlyFileTest()
        {
            var utils = new Utils(SolrResultsProviderMocks.GetIndexedResultMock("something.else", MissingType.All));

            Assert.AreEqual(IndexedFlag.Indexed , utils.GetIndexedFlag("something.else"));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagIndexedEverythingTest()
        {
            var utils = new Utils(SolrResultsProviderMocks.GetIndexedResultMock("something.else", MissingType.None));

            Assert.AreEqual(IndexedFlag.Indexed | IndexedFlag.IndexedArtist | IndexedFlag.IndexedAlbum | IndexedFlag.IndexedTitle |IndexedFlag.IndexedLyrics, utils.GetIndexedFlag("something.else"));
        }
    }
}
